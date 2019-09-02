using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Spazdun: Node
    {
        public Expression Exp { get; set; }
        public Spazdun(List<Token> exp)
        {
            if (exp != null)
                Exp = new Expression(exp);
        }

        public override string GetInfo()
        {
            return "Spazdun ";
        }
    }
}
