using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Declaration: Node
    {
        public VarType Type { get; set; }
        public string VarName { get; set; }
        public Expression Exp { get; set; }
        public Declaration(VarType type, string varName, List<Token> exp)
        {
            Type = type;
            VarName = varName;
            if(exp!=null)
                Exp = new Expression(exp);
        }

        public override string GetInfo()
        {
            return "Dec " + VarName;
        }
    }
}
