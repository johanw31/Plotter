using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

namespace Plotter
{
    internal class GcodeHandler
    {
        public string GcodeFilePath = "";       //Pfad des Gcode Files
        private double StepResolution = 80;    // steps pro mm
        private int LinearSpeed = 0;            //Geschwindigkeit bei Fahrt mit Stift oben
        private double[] LivePos = { 0, 0 };    // aktuelle Position

        public GcodeHandler(string GcodeFilePath) 
        {
        this.GcodeFilePath = GcodeFilePath;
        }
        public void Set_StepResolution(double StepResolution) //steps pro mm
        {
            this.StepResolution = StepResolution;
        }
        public void Set_LinearSpeed(int LinearSpeed) //steps pro mm
        {
            this.LinearSpeed = LinearSpeed;
        }
        public double[] GcodeKoord(string Gcode) //x,y,z und Geschwindigkeiten aus String in double formatieren
        {
            int posX = 0; int posY = 1; int posZ = 2; int speed = 3;
            int StartIndex_x = 1, StartIndex_y, StartIndex_speed, EndIndex_x, EndIndex_y;
            string StringX, StringY;
            string StringSpeed = LinearSpeed.ToString();
            double[] GoTo = { 0, 0, 0, 0 };
            if(Gcode.Contains("G00")) 
            { 
                GoTo[posZ] = 0;
            }
            if(Gcode.Contains("G01")) 
            { 
                GoTo[posZ] = 1;
            }
            StringX = Gcode.Substring(Gcode.IndexOf('X'));
            EndIndex_x = StringX.IndexOf('Y') - 1;
            StartIndex_y = StringX.IndexOf('Y') + 1;
            EndIndex_y = StringX.IndexOf('F') - 1;
            if (EndIndex_y < 0)
            {
                EndIndex_y = StartIndex_y;
            }
            StartIndex_speed = StringX.IndexOf('F') + 1;
            StringY = StringX.Substring(StartIndex_y);
            if (!Gcode.Contains("G00"))
            {
                StringY = StringX.Substring(StartIndex_y/*, EndIndex_y - StartIndex_y*/);
                StringSpeed = StringX.Substring(StartIndex_speed/*, EndIndex_speed - StartIndex_speed*/);
            }
            StringX = StringX.Substring(StartIndex_x, EndIndex_x - StartIndex_x);
            GoTo[posX] = Convert.ToDouble(double.Parse(StringX, System.Globalization.CultureInfo.InvariantCulture));
            GoTo[posY] = Convert.ToDouble(double.Parse(StringY, System.Globalization.CultureInfo.InvariantCulture));
            GoTo[speed] = 300;//Convert.ToDouble(double.Parse(StringSpeed, System.Globalization.CultureInfo.InvariantCulture));

            return GoTo;
        }
        /*
         * @Get_Steps gibt die benötigten Schritte von der ist-Position bis zur nächsten Position, die Soll Geschwindigkeit und die absolute anzufahrende Position zurück
         * @return: 1: X Steps; 2: Y Steps; 3: Z Position; 4: Geschwindigkeit; 5: Soll Koordinate X; 6: Soll Koordinate Y
        */
        public List<int[]> Get_Steps()
        {
            List<int[]> ListOfSteps = new List<int[]>();
            double[] LastPos = { 0, 0, 0, 0 };
            double[] Positions;
            foreach (var myString in File.ReadAllLines(this.GcodeFilePath))
            {
                Console.WriteLine(myString);
                if (myString.StartsWith("G00") | myString.StartsWith("G01"))
                {
                    Positions = GcodeKoord(myString); //0:X 1:Y 2:Z 3:Geschwindigkeit
                    Positions[0] -= LastPos[0];
                    Positions[1] -= LastPos[1];
                    Positions[0] *= StepResolution;
                    Positions[1] *= -StepResolution;
                    LastPos = GcodeKoord(myString);
                    int[] Data = new int[]
                        {
                        (int)Positions[0],
                        (int)Positions[1],
                        (int)Positions[2],
                        (int)Positions[3],
                        (int)LastPos[0],
                        (int)LastPos[1],
                        };
                    ListOfSteps.Add(Data);
                }
                if (myString.StartsWith("M2"))
                {
                    int[] Data = new int[]
                        {
                        (int)-LastPos[0],
                        (int)-LastPos[1],
                        0,
                        300,
                        0,
                        0,
                        };
                    ListOfSteps.Add(Data);
                }
            }
            return ListOfSteps;
        }
    }
}
