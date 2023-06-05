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

namespace Plotter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Automatik AutoWindow = new Automatik();
        private Hand HandWindow = new Hand();
        public MainWindow()
        {
            InitializeComponent();
            BtnAuto.Click += new RoutedEventHandler(this.BtnAuto_Click);
            BtnHand.Click += new RoutedEventHandler(this.BtnHand_Click);
        }

        private void BtnAuto_Click(Object sender, EventArgs e)
        {
            Main.Content = AutoWindow; ;
        }

        private void BtnHand_Click(Object sender, EventArgs e)
        {
            Main.Content = HandWindow;
        }
    }
}
