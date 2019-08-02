using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Assignment:Node
    {
        public string VarName { get; set; }
        public Expression VarIndex { get; set; }

        public Expression Exp { get; set; }
        public Assignment(string varName, List<Token> exp)
        {
            VarName = varName;
            Exp = new Expression(exp,false);
        }
        public override string GetInfo()
        {
            return "Assignment " + VarName;
        }
    }
}
