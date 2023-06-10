using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private readonly string HOST = "localhost";
        private readonly int PORT = 4223;
        private readonly string UID_Stepper_X = "26Nv";
        private readonly string UID_Stepper_Y = "26NW";
        private readonly string UID_Servo_Z = "SFU";
        private BrickletSilentStepperV2 Stepper_X;
        private BrickletSilentStepperV2 Stepper_Y;
        private IPConnection ipcon;
        private BrickletServoV2 Servo_Z;
        private readonly Automatik AutoWindow;
        private readonly Hand HandWindow;
        
        /*private readonly string HOST = "localhost";
        private readonly int Port = 4223;
        private readonly string UID_Stepper_X = "XXX";
        private readonly string UID_Stepper_Y = "XXX";
        private readonly string UID_Servo_Z = "XXX";
       // readonly IPConnection ipcon = new IPConnection();*/
        public MainWindow()
        {
            InitializeComponent();
            if (ipcon == null) ipcon = new IPConnection();
            if (ipcon.GetConnectionState() != 1)ipcon.Connect(HOST, PORT); // Connect to brickd 
            InitStepper_X();
            InitStepper_Y();
            InitServo_Z();

            if (HandWindow == null) HandWindow = new Hand(Stepper_X, Stepper_Y, Servo_Z, ipcon);
            if (AutoWindow == null) AutoWindow = new Automatik(Stepper_X, Stepper_Y, Servo_Z, ipcon);
            BtnAuto.Click += new RoutedEventHandler(this.BtnAuto_Click);
            BtnHand.Click += new RoutedEventHandler(this.BtnHand_Click);
            BtnConnect.Click += new RoutedEventHandler(this.BtnConnect_Click);
            
        }
        private void InitStepper_X()
        {
            this.Stepper_X = new BrickletSilentStepperV2(UID_Stepper_X, ipcon); // Create device object
            this.Stepper_X.SetMotorCurrent(500); // 800 mA
            this.Stepper_X.SetStepConfiguration(BrickletSilentStepperV2.STEP_RESOLUTION_256, true); // 1/8 steps (interpolated)
            this.Stepper_X.SetMaxVelocity(30000); // Velocity 2000 steps/s

            // Slow acceleration (500 steps/s^2),
            // Fast deacceleration (5000 steps/s^2)
            this.Stepper_X.SetSpeedRamping(0, 0);
            this.Stepper_X.SetEnabled(true); // Enable motor power
        }
        private void InitStepper_Y()
        {

            this.Stepper_Y = new BrickletSilentStepperV2(UID_Stepper_Y, ipcon); // Create device object
            this.Stepper_Y.SetMotorCurrent(500); // 800 mA
            this.Stepper_Y.SetStepConfiguration(BrickletSilentStepperV2.STEP_RESOLUTION_256, true); // 1/8 steps (interpolated)
            this.Stepper_Y.SetMaxVelocity(30000); // Velocity 2000 steps/s

            // Slow acceleration (500 steps/s^2),
            // Fast deacceleration (5000 steps/s^2)
            this.Stepper_Y.SetSpeedRamping(0, 0);
            this.Stepper_Y.SetEnabled(true); // Enable motor power
        }
        private void InitServo_Z()
        {
            this.Servo_Z = new BrickletServoV2(UID_Servo_Z, ipcon);
            this.Servo_Z.SetPulseWidth(0, 950, 1950);
            this.Servo_Z.SetPeriod(0, 1);

        }
        private void BtnConnect_Click(Object sender, EventArgs e)
        {
            //if (ipcon.GetConnectionState() == 0) ipcon.Connect(HOST, Port);
            //if (ipcon.GetConnectionState() > 0) BtnConnect.Background = Brushes.Green;
        }
        private void BtnAuto_Click(Object sender, EventArgs e)
        {
            //if (ipcon.GetConnectionState() > 0) Main.Content = AutoWindow;
            Main.Content = AutoWindow;
        }

        private void BtnHand_Click(Object sender, EventArgs e)
        {
            //if (ipcon.GetConnectionState() > 0) Main.Content = HandWindow;
            Main.Content = HandWindow;
        }
    }
}
