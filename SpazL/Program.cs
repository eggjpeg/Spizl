using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Program
    {
        static void Main(string[] args)
        {
            //SpazL.Run(args[0]);
            string trace = SpazL.Run("spazl/Quicksort.spaz");

            Console.ReadLine();
        }
    }
}
