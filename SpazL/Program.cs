using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpazL
{
    class Program
    {
        static void Main(string[] args)
        {
            //SpazL.Run(args[0]);
            SpazL spazl = new SpazL("spazl/Quicksort.spaz");
            spazl.Run();

            Console.ReadLine();
        }
    }
}
