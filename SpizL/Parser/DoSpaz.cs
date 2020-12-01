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
    /// 3. dospaz int item col
    /// 
    /// lint col
    /// spadd(col,2)
    /// spadd(col,3)
    /// 
    /// dospaz int item col
    ///     sprint(item).
    ///     
    /// </summary>
    /// 

    public enum DoSpazType
    {
        Infinite = 1,
        While = 2,
        Foreach = 3
    }

    class DoSpaz : Node
    {
        
        //Either starting declaration or condition
        public Expression Exp { get; set; }

        public DoSpazType Type { get; set; }

        public TokenType ItemType { get; set; }
        public string ItemName { get; set; }
        public string Collection { get; set; }




        private bool HasOp(List<Token> list)
        {
            foreach (var item in list)
                if (item.IsOp())
                    return true;               
            return false;
        }

        public DoSpaz(List<Token> e)
        {
            this.SetNewParent = true;
            if (e == null || e.Count == 0) // Type 1
            {
                Exp = null;
                Type = DoSpazType.Infinite;
            }
            else if (HasOp(e)) //Type 2
            {
                Exp = new Expression(e);
                Type = DoSpazType.While;
            }
            else if(e.Count == 3) //Type 3
            {
                ItemType = e[0].Type;
                if(!(ItemType == TokenType.Int || ItemType == TokenType.Str))
                    throw new Exception("SPAZ your dospaz variable has to be type int or string SPAZ");
                ItemName = e[1].Value;
                Collection = e[2].Value;
                Type = DoSpazType.Foreach;
            }
            else
            {
                throw new Exception("SPAZ incorrect dospaz type, type 4 diabetes ");
            }
        }
        public override string ToTreeString(int indent)
        {
            switch(Type)
            {
                case DoSpazType.Infinite: return "Infinite Spaz...";
                case DoSpazType.While: return "DoSpaz " + Token.ToString(Exp.Exp);
                case DoSpazType.Foreach: return "DoSpaz " + ItemType.ToString() + " " + ItemName + " " + Collection;
            }
            return "";
        }

    }
}
