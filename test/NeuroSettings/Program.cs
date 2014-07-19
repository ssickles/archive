using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NeuroSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MatchingThresholdFromString(".1%"));
            Console.WriteLine(MatchingThresholdToString(50));
            Console.WriteLine(MaximalRotationFromDegrees(200));
            Console.WriteLine(MaximalRotationToDegrees(71));
            Console.WriteLine(QualityFromPercent(39));
            Console.WriteLine(QualityToPercent(100));
            Console.ReadLine();
        }
    
        public static string MatchingThresholdToString(int value)
        {
            double p = -value / 12.0;
            return string.Format(string.Format("{{0:P{0}}}", Math.Max(0, (int)Math.Ceiling(-p) - 2)), Math.Pow(10, p));
        }
    
        public static int MatchingThresholdFromString(string value)
        {
            double p = Math.Log10(Math.Max(double.Epsilon, Math.Min(1,
                double.Parse(value.Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")) / 100)));
            return Math.Max(0, (int)Math.Round(-12 * p));
        }

        public static int MaximalRotationToDegrees(byte value)
        {
            return (2 * value * 360 + 256) / (2 * 256);
        }

        public static byte MaximalRotationFromDegrees(int value)
        {
            return (byte)((2 * value * 256 + 360) / (2 * 360));
        }

        public static int QualityToPercent(byte value)
        {
            return (2 * value * 100 + 255) / (2 * 255);
        }

        public static byte QualityFromPercent(int value)
        {
            return (byte)((2 * value * 255 + 100) / (2 * 100));
        }
    }
}
