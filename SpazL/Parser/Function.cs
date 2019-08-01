using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Function : Node
    {
        public string Name {get; set;}
        public Expression Exp { get; set; }
    
        public Function(List<Token> e)
        {
            this.Name = e[0].Value;
            Exp = new Expression(e, false);
        }
        public override string GetInfo()
        {
            return "Func "+ Name ;
        }
    }
}
