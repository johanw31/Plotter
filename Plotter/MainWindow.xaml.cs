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
        private string GcodeName = "";
        private string Gcode = "";
        GcodeHandler Test;
        public MainWindow()
        {
            InitializeComponent();
            Importbtn.Click += new RoutedEventHandler(this.Importbtn_Click);
            Startbtn.Click += new RoutedEventHandler(this.Startbtn_Click);
        }

        private  void Importbtn_Click(Object sender,EventArgs e)
        {
            OpenFileDialog GcodeFile = new OpenFileDialog();
            if (GcodeFile.ShowDialog() == true)
            {
                this.GcodeName = GcodeFile.FileName;
                this.Gcode = File.ReadAllText(this.GcodeName);
                //GcodeText.Text = this.Gcode;
            }
        }
        private void Startbtn_Click(object sender,EventArgs e)
        {
            if (true/*GcodeName.Contains(".gcode")*/)
            {
                Test = new GcodeHandler(GcodeName);
                Test.Set_LinearSpeed(500);
                Test.Set_StepResolution(360);
                List<int[]> ttt = Test.Get_Steps();
                foreach (var print in ttt)
                {
                    GcodeText.Text += print[0].ToString() + " " + print[1].ToString() + " " + print[2].ToString() + " " + print[3].ToString() + " " + print[4].ToString() + " " + print[5].ToString() + "\n";
                }
            }
        }
    }
}
