using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Spazout : Node
    {
        public Expression Exp { get; set; }
        public Spazout(List<Token> exp)
        {
            if (exp != null && exp.Count > 0)
                Exp = new Expression(exp);
        }

        public override string ToTreeString(int indent)
        {
            return "Spazout ";
        }
    }
}
