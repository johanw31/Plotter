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
        private readonly Automatik AutoWindow = new Automatik();
        private readonly Hand HandWindow = new Hand();
        private readonly string HOST = "localhost";
        private readonly int Port = 4223;
        private readonly string UID_Stepper_X = "XXX";
        private readonly string UID_Stepper_Y = "XXX";
        private readonly string UID_Servo_Z = "XXX";
        readonly IPConnection ipcon = new IPConnection();
        public MainWindow()
        {
            InitializeComponent();
            BtnAuto.Click += new RoutedEventHandler(this.BtnAuto_Click);
            BtnHand.Click += new RoutedEventHandler(this.BtnHand_Click);
            BtnConnect.Click += new RoutedEventHandler(this.BtnConnect_Click);
        }
        private void BtnConnect_Click(Object sender, EventArgs e)
        {
            if (ipcon.GetConnectionState() == 0) ipcon.Connect(HOST, Port);
            if (ipcon.GetConnectionState() > 0) BtnConnect.Background = Brushes.Green;
        }
        private void BtnAuto_Click(Object sender, EventArgs e)
        {
            if (ipcon.GetConnectionState() > 0) Main.Content = AutoWindow;
        }

        private void BtnHand_Click(Object sender, EventArgs e)
        {
            if (ipcon.GetConnectionState() > 0) Main.Content = HandWindow;
        }
    }
}
