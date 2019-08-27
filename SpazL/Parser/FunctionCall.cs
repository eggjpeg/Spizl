﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class FunctionCall : Node
    {
        public string Name {get; set;}
        public Expression Exp { get; set; }
    
        public FunctionCall(List<Token> e)
        {
            if (e[0].Type == TokenType.Command)
                this.Name = e[0].SubType.ToString();
            else
                this.Name = e[0].Value;

            Exp = new Expression(e);
        }
        public override string GetInfo()
        {
            return "Func " + Name ;
        }
    }
}