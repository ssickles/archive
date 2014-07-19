using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedSick.Data.FantasyBaseball
{
    public static partial class FantasyBaseball
    {
        public static double? WHIP(int? Player, int? OUTS, int? H, int? BB)
        {
            if (Player == null || OUTS.GetValueOrDefault(0) == 0)
            {
                return null;
            }
            else
            {
                return (((int)H + (int)BB) / ((double)OUTS / 3));
            }
        }

        public static double? ERA(int? Player, int? OUTS, int? ER)
        {
            if (Player == null || OUTS.GetValueOrDefault(0) == 0)
            {
                return null;
            }
            else
            {
                return ((int)ER / ((double)OUTS / 27));
            }
        }

        public static double? K9(int? Player, int? OUTS, int? SO)
        {
            if (Player == null || (int)OUTS.GetValueOrDefault(0) == 0)
            {
                return null;
            }
            else
            {
                return ((int)SO / ((double)OUTS / 27));
            }
        }

        public static string IP(int? Player, int? OUTS)
        {
            if (Player == null)
            {
                return string.Empty;
            }
            else
            {
                int remainder = 0;
                int quotient = Math.DivRem((int)OUTS.GetValueOrDefault(0), 3, out remainder);
                string result = quotient.ToString();
                if (remainder != 0)
                {
                    result = result + "." + remainder;
                }
                return result;
            }
        }

        public static double? OBP(int? Player, int? AB, int? BB, int? HBP, int? SF, int? H)
        {
            if (Player == null || ((int)AB.GetValueOrDefault(0) + (int)BB.GetValueOrDefault(0) + (int)HBP.GetValueOrDefault(0) + (int)SF.GetValueOrDefault(0)) == 0)
            {
                return null;
            }
            else
            {
                return ((int)H + (int)BB + (int)HBP) / ((int)AB + (int)BB + (int)HBP + (double)SF);
            }
        }

        public static double? SLG(int? Player, int? AB, int? H, int? _2B, int? _3B, int? HR)
        {
            if (Player == null || AB.GetValueOrDefault(0) == 0)
            {
                return null;
            }
            else
            {
                return ((int)H + (int)_2B + 2 * (int)_3B + 3 * (int)HR) / (double)AB;
            }
        }

        public static double? AVG(int? Player, int? AB, int? H)
        {
            if (Player == null || AB.GetValueOrDefault(0) == 0)
            {
                return null;
            }
            else
            {
                return Math.Round((int)H.Value / (double)AB.Value, 3);
            }
        }
    }
}
