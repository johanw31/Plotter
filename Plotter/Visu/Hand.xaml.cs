using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
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

namespace Plotter
{
    /// <summary>
    /// Interaktionslogik für Hand.xaml
    /// </summary>
    public partial class Hand : Page
    {
        public int test = 0;
       // private readonly string HOST = "localhost";
       // private readonly int PORT = 4223;
       // private readonly string UID_Stepper_X = "26Nv";
       // private readonly string UID_Stepper_Y = "26NW";
        //private readonly string UID_Servo_Z = "SFU";
        private BrickletSilentStepperV2 Stepper_X;
        private BrickletSilentStepperV2 Stepper_Y;
        private IPConnection ipcon;
        private BrickletServoV2 Servo_Z;
        public Hand(BrickletSilentStepperV2 Stepper_X, BrickletSilentStepperV2 Stepper_Y, BrickletServoV2 Servo_Z, IPConnection ipcon)
        {
            InitializeComponent();
            this.Stepper_X = Stepper_X;
            this.Stepper_Y = Stepper_Y;
            this.Servo_Z = Servo_Z;
            this.ipcon = ipcon;
            BtnHandUp.Click += new RoutedEventHandler(this.BtnHandUp_MouseDown);
            BtnHandDown.Click += new RoutedEventHandler(this.BtnHandDown_MouseDown);
            BtnHandLeft.Click += new RoutedEventHandler(this.BtnHandLeft_MouseDown);
            BtnHandRight.Click += new RoutedEventHandler(this.BtnHandRight_MouseDown);
            BtnZDown.Click += new RoutedEventHandler(this.BtnZDown_MouseDown);
        }
        private void BtnHandUp_MouseDown(object sender, EventArgs e) 
        {
            testbox.Text += "HI";
            this.Stepper_Y.SetSteps(-50000); // Drive 60000 steps forward
        }
        private void BtnHandDown_MouseDown(object sender, EventArgs e)
        {
            this.Stepper_Y.SetSteps(50000); // Drive 60000 steps forward
        }
        private void BtnHandLeft_MouseDown(object sender, EventArgs e)
        {
            this.Stepper_X.SetSteps(-50000); // Drive 60000 steps forward

        }
        private void BtnHandRight_MouseDown(object sender, EventArgs e)
        {
            this.Stepper_X.SetSteps(50000); // Drive 60000 steps forward
        }
        private void BtnZDown_MouseDown(object sender, EventArgs e)
        {
            if (!this.Servo_Z.GetEnabled(0)) this.Servo_Z.SetEnable(0, true);
            else this.Servo_Z.SetEnable(0, false);
        }
    }
}
