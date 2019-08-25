using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{

    class FunctionParam
    {
        public VarType Type { get; }
        public string Name { get;}

        public FunctionParam(VarType type, string name)
        {
            this.Name = name;
            this.Type = type;
        }

        public override string ToString()
        {
            return Type.ToString() + " " + Name;
        }

    }


    class FunctionDef : Node
    {
        public string Name { get;}
        public List<FunctionParam> Params { get;}
        public VarType ReturnType { get; }




        public FunctionDef(List<Token> expList)
        {
            this.Name = expList[0].Value;
            this.Params = new List<FunctionParam>();

            //myFun int a, int b : int
            for (int i = 1; i < expList.Count; i++)
            {
                Token t = expList[i];
                //farm parens
                if (t.Type == TokenType.Op && ((OpType)t.SubType == OpType.Oparen || (OpType)t.SubType == OpType.Cparen || (OpType)t.SubType == OpType.Comma))
                    continue;
                //If you hit a colon, then check for return type
                if (t.Type == TokenType.Op && (OpType)t.SubType == OpType.Colon)
                {
                    //Is there a return type?
                    if (expList.Count > i + 1 && expList[i + 1].Type == TokenType.Type)
                        this.ReturnType = (VarType)t.SubType;
                    else
                        this.ReturnType = VarType.Void;
                    break;
                }

                string name = expList[i + 1].Value;
                //here we expect a var type and a name 
                FunctionParam p = new FunctionParam((VarType)t.SubType, name);
                Params.Add(p);
                i++;
            }



        }
        public override string GetInfo()
        {
            string s =  "Func Def " + Name + "(";
            foreach (FunctionParam p in Params)
                s += p.ToString() + ",";
            s = s.Trim(',') + ')'; //MIND EXPLOSION
            s += ":" + ReturnType.ToString("G");
            return s;
        }
    }
}
