using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Util
    {
        public static bool IsDouble(char str)
        {
            return IsDouble(str.ToString());
        }

        public static bool IsDouble(string str)
        {
            return double.TryParse(str.ToString(), out double spaz);
        }



        public static Tuple<int, int> FindInnerPars(List<ExpNode> list, ExpNode par1, ExpNode par2)
        {
            int i1 = list.LastIndexOf(par1) == -1 ? int.MaxValue : list.LastIndexOf(par1);
            int i2 = list.IndexOf(par2, i1) == -1 ? int.MaxValue : list.IndexOf(par2, i1);
            var tuple = new Tuple<int, int>(i1, i2);
            return tuple;
        }
        public static Tuple<int, int> FindClosestPars(List<object> list, char par1, char par2, int i)
        {
            int i1 = list.IndexOf(par1, i) == -1 ? int.MaxValue : list.IndexOf(par1);
            int i2 = list.IndexOf(par2, i1) == -1 ? int.MaxValue : list.IndexOf(par2, i1);
            var tuple = new Tuple<int, int>(i1, i2);
            return tuple;
        }
        public static void PrintList(List<Tuple<double, double>> list)
        {
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine(list[i]);
        }

        public static Boolean IsNumber(string s)
        {
            Boolean value = true;
            foreach (Char c in s.ToCharArray())
            {
                value = value && Char.IsDigit(c);
            }

            return value;
        }

    }
}

