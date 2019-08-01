using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class SymbolTable : Dictionary<string, string>
    {
        public void Load(string spazsymbolfile)
        {
            using (StreamReader sr = new StreamReader(spazsymbolfile))
            {
                while (!sr.EndOfStream)
                {
                    string key, value;
                    string line = sr.ReadLine();
                    string[] linear = line.Split(' ');
                    if (linear.Length == 2)
                    {
                        key = linear[0];
                        value = linear[1];
                    }
                    else
                        key = value = linear[0];

                    this.Add(key.ToLower(), value.ToLower());
                }
            }
        }
    }
}
