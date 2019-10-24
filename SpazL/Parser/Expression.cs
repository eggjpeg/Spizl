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

        public Expression(List<Token> exp)
        {
            Exp = exp;
            ExpTree = BuildTree();
        }

        public Expression(ExpNode expNode)
        {
            ExpTree = expNode;
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
                if (list[startParenIndex - 1].Token.Type == TokenType.VarName || list[startParenIndex - 1].Token.IsCommand())
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
            if (list.Count == 0)
                return rlist;

            List<ExpNode> curList = new List<ExpNode>();
            foreach (ExpNode node in list)
            {
                if (node.Token.Type == TokenType.Comma)
                {
                    rlist.Add(curList);
                    curList = new List<ExpNode>();
                    continue;
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


            if (HasOpType(list, TokenType.Oparen))
                return ProcessParens(list);

            if (HasOpType(list, TokenType.Obrack))
                return ProcessBracks(list);


            var orderList = new List<List<TokenType>>();
            orderList.Add(new List<TokenType> { TokenType.Multiply, TokenType.Divide, TokenType.Mod });
            orderList.Add(new List<TokenType> { TokenType.Plus, TokenType.Minus });
            orderList.Add(new List<TokenType> { TokenType.LessThanEq, TokenType.Lessthan, TokenType.Morethan, TokenType.MoreThanEq });
            orderList.Add(new List<TokenType> { TokenType.Equal, TokenType.NotEq });

            foreach (List<TokenType> opSet in orderList)
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




        private int IndexOfO(List<ExpNode> list, TokenType type)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].Token.Type == type)
                        return i;
            throw new Exception("Could not find Opening Op. spz.");
        }

        private int IndexOfC(List<ExpNode> list, int oIndex, TokenType type)
        {
            for (int i = oIndex; i < list.Count; i++)
                if (list[i].Token.Type == type)
                    return i;
            throw new Exception("Could not find Closing Op. spz.");
        }

        public Tuple<int, int> FindInnerPars(List<ExpNode> list)
        {
            int i1 = IndexOfO(list,TokenType.Oparen);
            int i2 = IndexOfC(list, i1, TokenType.Cparen);
            return new Tuple<int, int>(i1, i2);
        }

        public Tuple<int, int> FindInnerBrackets(List<ExpNode> list)
        {
            int i1 = IndexOfO(list, TokenType.Obrack);
            int i2 = IndexOfC(list, i1, TokenType.Cbrack);
            return new Tuple<int, int>(i1, i2);
        }



        private bool HasOpType(List<ExpNode> list, TokenType type )
        {
            foreach (ExpNode node in list)
                if (node.Token.Type == type)
                    return true;
            return false;
        }

        private int FindIndexOfFirstOp(List<ExpNode> list, List<TokenType> ops)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ExpNode node = list[i];
                if (node.IsLeaf())
                    foreach (TokenType op in ops)
                            if (node.Token.Type == op)
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



  

        private void SetValue(Dictionary<ExpNode, object> valueDict, ExpNode node, object value)
        {
            if (valueDict.ContainsKey(node))
                valueDict[node] = value;
            else
                valueDict.Add(node, value);
        }

        private object GetValue(Dictionary<ExpNode, object> valueDict, ExpNode node)
        {
            if (valueDict.ContainsKey(node))
                return valueDict[node];
            return null;
        }


        private object Sub(State state, object expValue)
        {
            if (state.ContainsKey(expValue.ToString()))
                return state[expValue.ToString()].Value;
            else
                return expValue;
            
        }



        private object EvalCustomFunc(AST ast, string funcName, List<object> argList, State state)
        {
            Squirrel sq = new Squirrel(ast, argList, funcName);
            return sq.Traverse();
        }


        private object EvalFunc(AST ast, string funcName, List<object> argList, State state)
        {
            switch(funcName.ToLower())
            {
                case "sprint": return FunctionLib.Sprint(argList, state.Trace, state.IsTrace);
                case "add": return FunctionLib.Add(argList);
                case "mul": return FunctionLib.Mul(argList);
                case "splen": return FunctionLib.Splen(argList);
                case "spad": return FunctionLib.Spad(argList);
                case "spre": return FunctionLib.Spre(argList);
                case "spat": return FunctionLib.Spat(argList);
                default: return EvalCustomFunc(ast, funcName, argList, state);
            }
        }

        public object Eval(AST ast, State state)
        {
            Dictionary<ExpNode, object> valueDict = new Dictionary<ExpNode, object>();
            return Eval(ast, ExpTree, state, valueDict);
        }

        public object Eval(AST ast, ExpNode n, State state, Dictionary<ExpNode, object> valueDict)
        {
            if (n.Token.Type == TokenType.Const)
                return n.Token.Value;

            object v = GetValue(valueDict, n);
            if (v != null)
                return v;

            if(n.IsFunction)
            {
                List<object> argList = new List<object>();
                foreach(ExpNode arg in n.ChildList)
                {
                    object r = Eval(ast, arg, state, valueDict);
                    SetValue(valueDict,arg,r);
                    argList.Add(r);
                }

                v = EvalFunc(ast, n.FunctionName, argList, state);
                SetValue(valueDict, n, v);
                return v;
            }
            else if(n.IsListIndex)
            {
                //Sanity Check
                if (n.ChildList.Count > 1)
                    throw new Exception("List Index can only have 1 child spaz.");
                object index = Eval(ast, n.ChildList[0], state, valueDict);
                index = Sub(state, index);
                //n.ChildList[0].Value = index; //Sets the value of the Index
                SetValue(valueDict, n.ChildList[0], index);

                var list = (List<object>)state[n.ListName].Value;
                v = list[int.Parse(index.ToString())];
                return v;
            }

            if (n.Token.Type == TokenType.VarName)
            {
                v = n.Token.Value;
                v = Sub(state, v);
                return v;
            }


            //Sanity check                                                                                       
            if (!n.Token.IsOp())
                throw new Exception("Op Type expected.. spaz.");

            TokenType op = n.Token.Type;

            
            object lValue = Eval(ast, n.ChildList[0], state, valueDict);
            lValue = Sub(state, lValue);

 
            object rValue = Eval(ast, n.ChildList[1], state, valueDict);
            rValue = Sub(state, rValue);


            switch (op)
            {
                case TokenType.Plus:
                    v = ToInt(lValue) + ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Minus:
                    v = ToInt(lValue) - ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Divide:
                    v = ToInt(lValue) / ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Multiply:
                    v = ToInt(lValue) * ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Mod:
                    v = ToInt(lValue) % ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Equal:
                    v = ToString(lValue) == ToString(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.NotEq:
                    v = ToString(lValue) != ToString(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Lessthan:
                    v = ToInt(lValue) < ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.LessThanEq:
                    v = ToInt(lValue) <= ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.Morethan:
                    v = ToInt(lValue) > ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
                case TokenType.MoreThanEq:
                    v = ToInt(lValue) >= ToInt(rValue);
                    SetValue(valueDict, n, v);
                    return v;
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
