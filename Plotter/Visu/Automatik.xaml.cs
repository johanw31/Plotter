using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tinkerforge;
using static System.Net.Mime.MediaTypeNames;

namespace Plotter
{
    /// <summary>
    /// Interaktionslogik für Automatik.xaml
    /// </summary>
    public partial class Automatik : Page
    {
        private BrickletSilentStepperV2 Stepper_X;
        private BrickletSilentStepperV2 Stepper_Y;
        private IPConnection ipcon;
        private BrickletServoV2 Servo_Z;
        private string GcodeName = "";
        private string Gcode = "";
        GcodeHandler Test;
        public Automatik(BrickletSilentStepperV2 Stepper_X, BrickletSilentStepperV2 Stepper_Y, BrickletServoV2 Servo_Z, IPConnection ipcon)
        {
            InitializeComponent();
            this.Stepper_X = Stepper_X;
            this.Stepper_Y = Stepper_Y;
            this.Servo_Z = Servo_Z;
            this.ipcon = ipcon;

            Importbtn.Click += new RoutedEventHandler(this.Importbtn_Click);
            Startbtn.Click += new RoutedEventHandler(this.Startbtn_Click);
        }
        private void Importbtn_Click(Object sender, EventArgs e)
        {
            OpenFileDialog GcodeFile = new OpenFileDialog();
            if (GcodeFile.ShowDialog() == true)
            {
                this.GcodeName = GcodeFile.FileName;
                this.Gcode = File.ReadAllText(this.GcodeName);
                //GcodeText.Text = this.Gcode;
            }
        }
        private void Startbtn_Click(object sender, EventArgs e)
        {
            if (true/*GcodeName.Contains(".gcode")*/)
            {
                bool Temp;
                Test = new GcodeHandler(GcodeName);
                Test.Set_LinearSpeed(500);
                Test.Set_StepResolution(80.0);
                List<int[]> ttt = Test.Get_Steps();
                int counter = 0;
                do
                { 
                    int Stepx = this.Stepper_X.GetRemainingSteps();
                    int Stepy = this.Stepper_X.GetRemainingSteps();
                    if (Stepx == 0 & Stepy == 0)
                    {
                        int[] print = ttt[counter];
                        if (print[2] > 0) { Temp = true; }
                        else { Temp = false; }
                        Draw(print[0], print[1], Temp);
                        counter++;
                    } 
                } while (ttt.Count > counter);
                /*foreach (var print in ttt)
                {
                    //GcodeText.Text += print[0].ToString() + " " + print[1].ToString() + " " + print[2].ToString() + " " + print[3].ToString() + " " + print[4].ToString() + " " + print[5].ToString() + "\n";
                    if (print[2] > 0) { Temp = true; }
                    else { Temp = false; }
                    //GcodeText.Text += "-2";
                    Draw(print[0], print[1], Temp);
                    //GcodeText.Text += "0";
                    while (this.Stepper_X.GetRemainingSteps() > 0) { }
                    while (this.Stepper_Y.GetRemainingSteps() > 0) { }
                    //GcodeText.Text += "10";
                }*/
            }
        }
        private void Draw(int GoToX, int GoToY, bool GoToZ)
        {
            this.Servo_Z.SetEnable(0,GoToZ);
            Thread.Sleep(50);
            this.Stepper_X.SetSteps(GoToX);
            this.Stepper_Y.SetSteps(GoToY);
        }
    }

}
