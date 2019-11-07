using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            SpazL spazl = new SpazL("spazl/infinite.spaz");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            spazl.Run();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
