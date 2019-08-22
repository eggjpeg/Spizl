using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class FunctionLib
    {
        public static int Add(List<object> list)
        {
            int sum = 0;
            foreach(var item in list)
                sum += int.Parse(item.ToString());
            return sum;
        }

        public static int Mul(List<object> list)
        {
            int p = 1;
            foreach (var item in list)
                p *= int.Parse(item.ToString());
            return p;
        }


        public static int Sprint(List<object> list)
        {
            foreach (var item in list)
                Console.Write(item);
            Console.WriteLine();
            return 0;
        }

        public static int Splen(List<object> list)
        {
            var spazList = list[0];
            if (!(spazList is List<object>))
                throw new Exception("invalid list type spaz.");
            return ((List<object>)spazList).Count;
        }


        public static int Spad(List<object> list)
        {
            var lst = list[0];
            if (!(lst is List<object>))
                throw new Exception("invalid list type spaz.");

            var spazList = lst as List<object>;
            for (int i=1;i<list.Count;i++)
                spazList.Add(list[i]);

            return 0;
        }

        public static int Spre(List<object> list)
        {
            var lst = list[0];
            if (!(lst is List<object>))
                throw new Exception("invalid list type spaz.");
            var spazList = lst as List<object>;

            if (list.Count > 2)
               spazList.RemoveRange(int.Parse(list[1].ToString()), int.Parse(list[2].ToString()));
            else
               spazList.RemoveAt(int.Parse(list[1].ToString()));

            return 0;
        }

        public static string Spat(List<object> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
                sb.Append(item);
            return sb.ToString();
        }

    }
}
