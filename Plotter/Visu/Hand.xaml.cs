using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
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
    /// Interaktionslogik für Hand.xaml
    /// </summary>
    public partial class Hand : Page
    {
        public int test = 0;
        public Hand()
        {
            InitializeComponent();
            BtnHandUp.Click += new RoutedEventHandler(this.BtnHandUp_MouseDown);
        }
        private void BtnHandUp_MouseDown(object sender, EventArgs e) 
        {
            test++;
            testbox.Text = test.ToString();
            
        }
    }
}
