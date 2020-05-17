using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Compiler
    {

        //        spaz:
        //int j = 2
        //int i = 2 + 3 - (5 * add(2 * 4, 3 - 3, mul(4 * 2, j * 3 * j)))
        //sprint(i)
        //spif i != 9
        //    sprint(i + 1)..

        public string ToMSIL(List<Instruction> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Instruction ins in list)
                sb.AppendLine(ins.ToMSIL());
            return sb.ToString();
        }

        public string Compile(Expression exp)
        {
            List<Instruction> list = new List<Instruction>();
            Compile(exp.ExpTree, list);

            string result = ToMSIL(list);
            return result;
        }

          //ldc.i4 2
          //ldc.i4 5
          //ldc.i4 3
          //mul
          //ldc.i4 2
          //mul
          //ldc.i4 6
          //ldc.i4 7
          //ldc.i4 3
          //mul
          //add
          //add
          //add

        private Instruction MapTokenToInstruction(Token token)
        {
            Instruction ins = new Instruction();
            switch (token.Type)
            {
                case TokenType.Const:
                    ins.Type = InstructionType.Stack_Push_ConstInt32;
                    ins.Value = token.Value;
                    break;
                case TokenType.Minus:
                    ins.Type = InstructionType.Arith_Sub;
                    break;
                case TokenType.Plus:
                    ins.Type = InstructionType.Arith_Add;
                    break;
                case TokenType.Multiply:
                    ins.Type = InstructionType.Arith_Mul;
                    break;
                case TokenType.Divide:
                    ins.Type = InstructionType.Arith_Div;
                    break;
            }
            return ins;

        }

        private void Compile(ExpNode expNode, List<Instruction> list)
        {

            if (expNode.ChildList != null)
            {
                foreach (ExpNode child in expNode.ChildList)
                    Compile(child, list);
            }

            Instruction i = MapTokenToInstruction(expNode.Token);
            list.Add(i);


        }


    }
}
