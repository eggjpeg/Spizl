﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Spelzif:Node
    {
        public Expression Exp { get; set; }

        public Spelzif(List<Token> e)
        {
            this.SetNewParent = true;
            Exp = new Expression(e);
        }
        public override string GetInfo()
        {
            return "Spelzif " + Token.ToString(Exp.Exp);
        }
    }
}
