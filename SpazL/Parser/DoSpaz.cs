using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    /// <summary>
    /// 1. dospaz
    /// 2. dospaz i>2
    /// 
    /// ilist col
    /// col.add(2)
    /// col.add(3)
    /// 
    /// dospaz item, col, i
    ///     sprint item.
    ///     
    /// </summary>
    class DoSpaz: Node
    {
        //Either starting declaration or condition
        public Expression Exp { get; set; }

        public DoSpaz(List<Token> e)
        {
            if (e == null || e.Count == 0)
                Exp = null;
            else
                Exp = new Expression(e);
                
        }
        public override string GetInfo()
        {
            if (Exp == null)
                return "Infinite Spaz...";
            else
                return "DoSpaz " + Token.ToString(Exp.Exp);
        }

    }
}
