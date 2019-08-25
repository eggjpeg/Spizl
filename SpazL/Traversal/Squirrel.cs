using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    public enum TraverseMode
    {
        Interpret,
        Compile
    }

    class Squirrel
    {
        private State state = new State();
        private TraverseMode mode;
        private AST ast;

        
        public string GetTrace()
        {
            return state.Trace.ToString();
        }


        public Squirrel(AST ast, TraverseMode mode, bool trace)
        {
            this.mode = mode;
            this.ast = ast;
            this.state.IsTrace = trace;
        }


        public void Traverse()
        {
            Traverse(ast.Children[0], false);
        }

        private void Traverse(Node node, bool conCompleted)
        {
            if(node is Function)
            {
                if ((node as Function).Name == "spazdun")
                    return;
                (node as Function).Exp.Eval(state);
            }
            else if (node is Declaration)
            {
                Declaration d = (node as Declaration);

                object r = null;
                if (d.Exp !=null)
                    r = d.Exp.Eval(state);

                VarState vs = new VarState(d.VarName, d.Type, r);
                state.Add(vs.Name, vs);
            }
            else if (node is Assignment)
            {
                Assignment a = (node as Assignment);
                object r = a.RightExpression.Eval(state);
                if (a.IsListIndexAssignment())
                {
                   
                    object leftIndex = a.LeftIndexExpression.Eval(state);
                    int i = int.Parse(leftIndex.ToString());
                    var list = (List<object>)state[a.VarName].Value;
                    list[i] = r;
                }
                else
                {
                    state[a.VarName].Value = r;
                }

            }
            else if (node is Spif)
            {
                Spif spif = (node as Spif);
                object r = spif.Exp.Eval(state);
                if (!(r is bool))
                    throw new Exception("SPAZ spif must evaluate to bool SPAZ");
                bool rb = (bool)r;
                if (rb)
                {
                    if (node.Children.Count == 0)
                        throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS IF STATEMENT");
                    Traverse(node.Children[0], false);
                    conCompleted = true;
                }
                else
                    conCompleted = false;
            }
            else if(node is Spelzif)
            {
                Spelzif spelzIf = (node as Spelzif);
                var prevSib = GetPrevChild(node);
                if (!(prevSib is Spif || prevSib is Spelzif))
                    throw new Exception("spaz cant have else if without previous spif or spelzif SPAZ");
                if (!(conCompleted))
                {
                    object r = spelzIf.Exp.Eval(state);
                    if (!(r is bool))
                        throw new Exception("SPAZ spelzif must evaluate to bool SPAZ");
                    bool rb = (bool)r;
                    if (rb)
                    {
                        if (node.Children.Count == 0)
                            throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS SPELZIF STATEMENT");
                        Traverse(node.Children[0], false);
                        conCompleted = true;
                    }
                }
            }
            else if(node is Spelz)
            {
                Spelz spelz = (node as Spelz);
                var prevSib = GetPrevChild(node);
                if (!(prevSib is Spif || prevSib is Spelzif))
                    throw new Exception("spaz cant have spelz without previous spif or elseif SPAZ");
                if (!(conCompleted))
                {
                     if (node.Children.Count == 0)
                            throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS SPELZ STATEMENT");
                        Traverse(node.Children[0], false);
                    
                }
            }
            else if (node is DoSpaz)
            {
                DoSpaz loop = (node as DoSpaz);
                object r = loop.Exp.Eval(state);
                if (!(r is bool))
                    throw new Exception("SPAZ dospaz must evaluate to bool SPAZ");
                bool rb = (bool)r;
                if (rb)
                {
                    if (node.Children.Count == 0)
                        throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS DOSPAZ STATEMENT");
                    Traverse(node.Children[0], false);
                    return;
                }
                else
                    conCompleted = false;
            }
            //Regular execution
            Node next = GetNextChild(node);
            if (next != null) 
                Traverse(next, conCompleted);
      
        }
        private Node GetNextChild(Node n)
        {
            if (n == null || n.Parent == null)
                return null;

            for (int i = 0; i < n.Parent.Children.Count - 1; i++)
                if(n.Id == n.Parent.Children[i].Id)
                    return n.Parent.Children[i + 1];

            //If you are here you are looking at the last guy in that branch
            //if (n.Parent is DoSpaz)
            //    return n.Parent;
                
            return null;
        }

        private Node GetPrevChild(Node n)
        {
            for (int i = 1; i < n.Parent.Children.Count; i++)
                if (n.Id == n.Parent.Children[i].Id)
                    return n.Parent.Children[i - 1];
            return null;
        }

        /*
         spaz: replace vars with values♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿
         spaz: fix naive flow into real ♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿♿
         */
    }
}
