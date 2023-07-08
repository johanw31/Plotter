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
        private async void Startbtn_Click(object sender, EventArgs e)
        {
            Task<int> task = StartTask(Test, GcodeName, Stepper_X, Stepper_Y, Servo_Z);
            int result = await task;
            Console.WriteLine($"Task complete with result: {result}");
        }

        static async Task<int> StartTask(GcodeHandler Test, string GcodeName, BrickletSilentStepperV2 Stepper_X, BrickletSilentStepperV2 Stepper_Y, BrickletServoV2 Servo_Z)
        {
            if (true/*GcodeName.Contains(".gcode")*/)
            {
                bool Temp;
                int Stepx, Stepy;
                Test = new GcodeHandler(GcodeName);
                Test.Set_LinearSpeed(500);
                Test.Set_StepResolution(1280.0);
                List<int[]> ttt = Test.Get_Steps();
                int counter = 0;
                do
                {
                    Stepx = Stepper_X.GetRemainingSteps();
                    Stepy = Stepper_Y.GetRemainingSteps();
                    int[] print = ttt[counter];
                    if (Stepx == 0 & Math.Abs(Stepy) == 0)
                    {

                        if (print[2] > 0) { Temp = true; }
                        else { Temp = false; }
                        Draw(print[0], print[1], Temp, Stepper_X,Stepper_Y,Servo_Z);
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
            //await Task.Delay(20);
            return 1;
        }
        private static void Draw(int GoToX, int GoToY, bool GoToZ, BrickletSilentStepperV2 Stepper_X, BrickletSilentStepperV2 Stepper_Y, BrickletServoV2 Servo_Z)
        {
            Servo_Z.SetEnable(0, GoToZ);
            if (GoToX > 1 | GoToX < -1) Stepper_X.SetSteps(GoToX);
            if (GoToY > 1 | GoToY < -1) Stepper_Y.SetSteps(GoToY);
        }
    }

}
