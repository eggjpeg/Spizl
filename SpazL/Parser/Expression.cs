using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{

    class Expression
    {
        public List<Token> Exp { get; set; }
        public ExpNode ExpTree { get; set; }

        public Expression(List<Token> exp, bool skipBuildTree)
        {
            Exp = exp;
            if (!skipBuildTree)
                ExpTree = BuildTree();
        }


        public void ReBuildTree()
        {
            ExpTree = BuildTree();
        }

        public ExpNode BuildTree()
        {
            List<ExpNode> list = new List<ExpNode>();
            foreach (Token t in Exp)
                list.Add(new ExpNode(t));

            var r = BuildTree(list);
            if (r.Count > 1)
                throw new Exception("There can only be 1 root, spaz");

            return r[0];
        }



        private bool IsFunction(int startParenIndex, List<ExpNode> list)
        {
            //If the previous ExpNode is a VarName, assume it's a function
            if(startParenIndex > 0)
                if (list[startParenIndex - 1].Token.Type == TokenType.VarName || list[startParenIndex - 1].Token.Type == TokenType.Command)
                    return true;
            return false;
        }

        private bool IsList(int startBrackIndex, List<ExpNode> list)
        {
            //If the previous ExpNode is a VarName, assume it's a list
            if (startBrackIndex > 0)
                if (list[startBrackIndex - 1].Token.Type == TokenType.VarName)
                    return true;
            return false;
        }


        private List<List<ExpNode>> SplitFuncArguments(List<ExpNode> list)
        {
            List<List<ExpNode>> rlist = new List<List<ExpNode>>();

            List<ExpNode> curList = new List<ExpNode>();
            foreach (ExpNode node in list)
            {
                if (node.Token.Type == TokenType.Op)
                {
                    if ((OpType)node.Token.SubType == OpType.Comma)
                    {
                        rlist.Add(curList);
                        curList = new List<ExpNode>();
                        continue;
                    }
                }
                curList.Add(node);
            }
            rlist.Add(curList);
            return rlist;
        }


        private List<ExpNode>  CreateInnerList(List<ExpNode> list, Tuple<int,int> bi)
        {
            var innerList = new List<ExpNode>();

            //Spaz code by mr.spaz
            for (int i = 0; i < list.Count; i++)
            {
                if (bi.Item1 < i && bi.Item2 > i)
                    innerList.Add(list[i]);
            }
            return innerList;
        }



        private List<ExpNode> ProcessParens(List<ExpNode> list)
        {
            //Parens
            var bi = FindInnerPars(list);
            var innerList = CreateInnerList(list, bi);

            if (IsFunction(bi.Item1, list))
            {
                ExpNode fNode = list[bi.Item1 - 1];
                fNode.IsFunction = true;
                fNode.Value = null; //KLUDGE

                List<List<ExpNode>> arglist = SplitFuncArguments(innerList);

                foreach (List<ExpNode> arg in arglist)
                {
                    List<ExpNode> argResult = BuildTree(arg);
                    if (argResult.Count > 1)
                        throw new Exception("Function Argument must resolve to a single ExpNode. spaz.");
                    fNode.AddChild(argResult[0]);
                }

                list.RemoveRange(bi.Item1, bi.Item2 - bi.Item1 + 1);
                return BuildTree(list);
            }
            else
            {
                list.RemoveRange(bi.Item1, bi.Item2 - bi.Item1 + 1);
                var ls = BuildTree(innerList);
                list.InsertRange(bi.Item1, ls);
                return BuildTree(list);
            }
        
        }


        private List<ExpNode> ProcessBracks(List<ExpNode> list)
        {
            //Parens
            var bi = FindInnerBrackets(list);
            var innerList = CreateInnerList(list, bi);

            if (IsList(bi.Item1, list))
            {
                ExpNode lNode = list[bi.Item1 - 1];
                lNode.IsListIndex = true;
                lNode.Value = null; //KLUDGE

                List<ExpNode> indexResult = BuildTree(innerList);
                if (indexResult.Count > 1)
                    throw new Exception("List Index must resolve to a single ExpNode. spaz.");
                lNode.AddChild(indexResult[0]);
                
                list.RemoveRange(bi.Item1, bi.Item2 - bi.Item1 + 1);
                return BuildTree(list);
            }
            else
            {
                list.RemoveRange(bi.Item1, bi.Item2 - bi.Item1 + 1);
                var ls = BuildTree(innerList);
                list.InsertRange(bi.Item1, ls);
                return BuildTree(list);
            }

        }


        private List<ExpNode> BuildTree(List<ExpNode> list)
        {
            if (list.Count == 1)
                return list;


            if (HasOpType(list, OpType.Oparen))
                return ProcessParens(list);

            if (HasOpType(list, OpType.Obrack))
                return ProcessBracks(list);


            var orderList = new List<List<OpType>>();
            orderList.Add(new List<OpType> { OpType.Multiply, OpType.Divide, OpType.Mod });
            orderList.Add(new List<OpType> { OpType.Plus, OpType.Minus });
            orderList.Add(new List<OpType> { OpType.LessThanEq, OpType.Lessthan, OpType.Morethan, OpType.MoreThanEq });
            orderList.Add(new List<OpType> { OpType.Equal, OpType.NotEq });

            foreach (List<OpType> opSet in orderList)
            {
                int smallestIndex = FindIndexOfFirstOp(list, opSet);
                if (smallestIndex > -1)
                {
                    DoOp(list, smallestIndex);
                    Shrink(list, smallestIndex);
                    return BuildTree(list);
                }
            }
            return list;
        }




        private int IndexOfO(List<ExpNode> list, OpType oToken)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].Token.SubType is OpType)
                    if ((OpType)list[i].Token.SubType == oToken)
                        return i;
            throw new Exception("Could not find Opening Op. spz.");
        }

        private int IndexOfC(List<ExpNode> list, int oIndex, OpType cToken)
        {
            for (int i = oIndex; i < list.Count; i++)
                if (list[i].Token.SubType is OpType)
                    if ((OpType)list[i].Token.SubType == cToken)
                        return i;
            throw new Exception("Could not find Closing Op. spz.");
        }

        public Tuple<int, int> FindInnerPars(List<ExpNode> list)
        {
            int i1 = IndexOfO(list,OpType.Oparen);
            int i2 = IndexOfC(list, i1, OpType.Cparen);
            return new Tuple<int, int>(i1, i2);
        }

        public Tuple<int, int> FindInnerBrackets(List<ExpNode> list)
        {
            int i1 = IndexOfO(list, OpType.Obrack);
            int i2 = IndexOfC(list, i1, OpType.Cbrack);
            return new Tuple<int, int>(i1, i2);
        }



        private bool HasOpType(List<ExpNode> list, OpType opType )
        {
            foreach (ExpNode node in list)
            {
                if (node.Token.SubType is OpType)
                    if ((OpType)node.Token.SubType == opType)
                        return true;
            }
            return false;
        }

        private int FindIndexOfFirstOp(List<ExpNode> list, List<OpType> ops)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ExpNode node = list[i];
                if (node.IsLeaf())
                    foreach (OpType op in ops)
                        if (node.Token.SubType is OpType)
                            if ((OpType)node.Token.SubType == op)
                                return i;
            }
            return -1;
        }


        void DoOp(List<ExpNode> list, int index)
        {
            list[index].AddChild(list[index - 1]);
            list[index].AddChild(list[index + 1]);
        }

        private void Shrink(List<ExpNode> list, int i)
        {
            list.RemoveAt(i + 1);
            list.RemoveAt(i - 1);
        }


        public object Eval(State state)
        {
            //KLUDGE
            ReBuildTree();
            return Eval(ExpTree, state);
        }

        private object Sub(State state, object expValue)
        {
            if (state.ContainsKey(expValue.ToString()))
                return state[expValue.ToString()].Value;
            else
                return expValue;
            
        }



        private object EvalFunc(string funcName, List<object> argList)
        {
            switch(funcName.ToLower())
            {
                case "add": return FunctionLib.Add(argList);
                case "mul": return FunctionLib.Mul(argList);
                case "sprint": return FunctionLib.Sprint(argList);
                case "splen": return FunctionLib.Splen(argList);
                default: throw new Exception("unknown func " + funcName + ". spaz");
            }
        }

        private object Eval(ExpNode n, State state)
        {
            if (n.IsInterpreted())
                return n.Value;

            
            if(n.IsFunction)
            {
                List<object> argList = new List<object>();
                foreach(ExpNode arg in n.ChildList)
                {
                    object r = Eval(arg, state);
                    r = Sub(state, r);
                    arg.Value = r;
                    argList.Add(r);
                }

                n.Value = EvalFunc(n.FunctionName, argList);
                return n.Value;
            }
            
            
            //Sanity Check
            if (n.Token.Type != TokenType.Op)
                throw new Exception("Op Type expected.. spaz.");

            OpType op = (OpType)n.Token.SubType;

            
            object lValue = Eval(n.ChildList[0], state);
            lValue = Sub(state, lValue);

 
            object rValue = Eval(n.ChildList[1], state);
            rValue = Sub(state, rValue);


            switch (op)
            {
                case OpType.Plus:
                    n.Value = ToInt(lValue) + ToInt(rValue);
                    return n.Value;
                case OpType.Minus:
                    n.Value = ToInt(lValue) - ToInt(rValue);
                    return n.Value;
                case OpType.Divide:
                    n.Value = ToInt(lValue) / ToInt(rValue);
                    return n.Value;
                case OpType.Multiply:
                    n.Value = ToInt(lValue) * ToInt(rValue);
                    return n.Value;
                case OpType.Mod:
                    n.Value = ToInt(lValue) % ToInt(rValue);
                    return n.Value;
                case OpType.Equal:
                    n.Value = ToString(lValue) == ToString(rValue);
                    return n.Value;
                case OpType.NotEq:
                    n.Value = ToString(lValue) != ToString(rValue);
                    return n.Value;
                case OpType.Lessthan:
                    n.Value = ToInt(lValue) < ToInt(rValue);
                    return n.Value;
                case OpType.LessThanEq:
                    n.Value = ToInt(lValue) <= ToInt(rValue);
                    return n.Value;
                case OpType.Morethan:
                    n.Value = ToInt(lValue) > ToInt(rValue);
                    return n.Value;
                case OpType.MoreThanEq:
                    n.Value = ToInt(lValue) >= ToInt(rValue);
                    return n.Value;
                default:
                    throw new NotImplementedException("spaz");
            }
        }

        private int ToInt(object v)
        {
            return int.Parse(v.ToString());
        }


        private string ToString(object v)
        {
            return v.ToString();
        }

    }
}
