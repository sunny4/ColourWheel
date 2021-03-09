using System;
using System.Drawing;
using System.IO;

namespace ColourWheel
{
    class Program
    {
        static int TotalColours;
        static int RedPoint;
        static int BluePoint;
        static int GreenPoint;

        static string fileName = @"C:\Users\sunny\Desktop\newColorWheel.bmp";

        static void Main(string[] args)
        {
            int numColours;
            do Console.WriteLine("Please specify how many parts to colour wheel: ");
            while (!int.TryParse(Console.ReadLine(), out numColours));

            TotalColours = numColours;
            MapColourPoints();
            float angle = CalculateAngle(TotalColours);

            Bitmap colourWheel = new Bitmap(200, 200);
            Graphics colourWheelGraphics = Graphics.FromImage(colourWheel);

            Rectangle rectangleBorder = new Rectangle(0, 0, 200, 200);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            colourWheelGraphics.FillRectangle(blackBrush, rectangleBorder);

            Rectangle rectangleWheel = new Rectangle(10, 10, 180, 180);

            float sweepFillAngle = angle * 0.85f;
            float startAngle = 270 - (sweepFillAngle / 2);
            int counter = 1;

            while (counter <= numColours)
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                if (counter <= BluePoint)
                {
                    int b = GetBlueFromRtoB(counter);
                    int r = GetRedFromRtoB(counter);
                    brush = new SolidBrush(Color.FromArgb(r, 0, b));
                }
                else if (counter <= GreenPoint)
                {
                    int g = GetGreenFromBtoG(counter);
                    int b = GetBlueFromBtoG(counter);
                    brush = new SolidBrush(Color.FromArgb(0, g, b));
                }
                else 
                {
                    int r = GetRedFromGtoR(counter);
                    int g = GetGreenFromGtoR(counter);
                    brush = new SolidBrush(Color.FromArgb(r, g, 0));
                }

                colourWheelGraphics.FillPie(brush, rectangleWheel, startAngle, sweepFillAngle);
                startAngle += angle;

                counter++;
            }

            Rectangle rectangleInnerHollow = new Rectangle(60, 60, 80, 80);
            colourWheelGraphics.FillPie(blackBrush, rectangleInnerHollow, startAngle, 360);

            colourWheel.Save(fileName);
            Console.ReadKey();
        }

        private static void MapColourPoints()
        {
            RedPoint = 1;
            BluePoint = TotalColours / 3;
            GreenPoint = (TotalColours / 3) * 2;
        }

        private static int GetBlueFromRtoB(int currentSegment)
        {
            if (currentSegment == BluePoint) return 255;
            if (currentSegment == RedPoint) return 0;

            int RtoBSegments = BluePoint - 1;
            int colourIncrement = 255 / RtoBSegments;

            return colourIncrement * (currentSegment - 1);
        }

        private static int GetRedFromRtoB(int currentSegment)
        {
            if (currentSegment == BluePoint) return 0;
            if (currentSegment == RedPoint) return 255;

            int RtoBSegments = BluePoint - 1;
            int colourIncrement = 255 / RtoBSegments;

            return colourIncrement * (RtoBSegments - (currentSegment - 1));
        }

        private static int GetGreenFromBtoG(int currentSegment)
        {
            if (currentSegment == GreenPoint) return 255;
            if (currentSegment == BluePoint) return 0;
            currentSegment -= BluePoint;

            int BtoGSegments = GreenPoint - (BluePoint);
            int colourIncrement = 255 / BtoGSegments;

            return colourIncrement * currentSegment;
        }

        private static int GetBlueFromBtoG(int currentSegment)
        {
            if (currentSegment == GreenPoint) return 0;
            if (currentSegment == BluePoint) return 255;
            currentSegment -= BluePoint;

            int BtoGSegments = GreenPoint - (BluePoint);
            int colourIncrement = 255 / BtoGSegments;

            return colourIncrement * (BtoGSegments - (currentSegment));
        }


        private static int GetGreenFromGtoR(int currentSegment)
        {
            currentSegment -= GreenPoint;

            int GtoRSegments = TotalColours - GreenPoint;
            int colourIncrement = 255 / GtoRSegments;

            return colourIncrement * (GtoRSegments - currentSegment);
        }

        private static int GetRedFromGtoR(int currentSegment)
        {
            if (currentSegment == GreenPoint) return 0;
            if (currentSegment == RedPoint) return 255;
            currentSegment -= GreenPoint;

            int BtoGSegments = TotalColours - (GreenPoint);
            int colourIncrement = 255 / BtoGSegments;

            return colourIncrement * currentSegment;
        }

        private static float CalculateAngle(int numSegments)
        {
            float angle = 360.0f / numSegments;
            return angle;
        }
    }
}
