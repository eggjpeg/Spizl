﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Squirrel
    {
        private State State = new State();
        private string funcName = "spaz";
        private AST ast;
        List<object> argList = new List<object>();


        public string GetTrace()
        {
            return State.Trace.ToString();
        }


        public Squirrel(AST ast)
        {
            this.ast = ast;
        }

        private void AddArgsToState(FunctionDef fd)
        {
            for (int i = 0; i < fd.Params.Count; i++)
            {
                //suspicious copying - needs testing
                VarState vs = new VarState(fd.Params[i].Name, fd.Params[i].Type, argList[i]);
                State.Add(fd.Params[i].Name, vs);
            }
            
        }

        //Used by functions
        public Squirrel(AST ast, List<object> argList, string funcName)
        {
            this.ast = ast;
            this.funcName = funcName;
            this.argList = argList;
        }
        private void ValidateArgs(FunctionDef fd)
        {
            if (fd.Params.Count > argList.Count)
                throw new Exception("SPAZ DOESNT HAVE ENOUGH ARGUMENTS");
            if (fd.Params.Count < argList.Count)
                throw new Exception("SPAZ HAS TOO MANY ARGUMENTS");
            //for (int i = 0; i < fd.Params.Count; i++)
            //{
            //    if (fd.Params[i].Type != argList[i].Type)
            //        throw new Exception("type mismatch SPAZ. Expecting a " + fd.Params[i].Type.ToString() + " but received: " + argList[i].Type.ToString());
            //}
        }
        private Node FindFunc(string name)
        {
            foreach (var item in ast.Children)
                if(item is FunctionDef)
                    if ((item as FunctionDef).Name == name)
                        return item;

            if (name == "spaz")
                throw new Exception("couldnt find function : " + name + " other than you");
            else
                throw new Exception("couldnt find function : " + name + " spaz");
        }

        public object Traverse()
        {
            Node spazFunc = FindFunc(funcName);
            return Traverse(spazFunc, false);
        }

        private bool swordOfKhali = false;
        private int spazoutLevel = 0;
        private bool spazoutInitiated = false;
        private object ret = null;

        private object Traverse(Node node, bool conCompleted)
        {
            if (swordOfKhali)
                return ret;
            
            if (node is FunctionDef)
            {
                FunctionDef fd = (node as FunctionDef);
                ValidateArgs(fd);
                AddArgsToState(fd);

                if (node.Children.Count == 0)
                    throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS DOSPAZ STATEMENT");
                
                return Traverse(node.Children[0], false);
            }
            else if (node is Spazdun)
            {
                if ((node as Spazdun).Exp == null)
                    return null;

                ret = (node as Spazdun).Exp.Eval(ast, State);
                swordOfKhali = true;
                return ret;
            }
            else if (node is Spazout)
            {
                spazoutInitiated = true;
                if ((node as Spazout).Exp == null)
                    return null;

                ret = (node as Spazout).Exp.Eval(ast, State);
                return ret;
            }
            else if (node is FunctionCall)
            {
                (node as FunctionCall).Exp.Eval(ast, State);
            }
            else if (node is Declaration)
            {
                Declaration d = (node as Declaration);

                object r = null;
                if (d.Exp !=null)
                    r = d.Exp.Eval(ast, State);

                VarState vs = new VarState(d.VarName, d.Type, r);
                State.Add(vs.Name, vs);
            }
            else if (node is Assignment)
            {
                Assignment a = (node as Assignment);
                object r = a.RightExpression.Eval(ast, State);
                if (a.IsListIndexAssignment())
                {
                   
                    object leftIndex = a.LeftIndexExpression.Eval(ast, State);
                    int i = int.Parse(leftIndex.ToString());
                    var list = (List<object>)State[a.VarName].Value;
                    list[i] = r;
                }
                else
                {
                    State[a.VarName].Value = r;
                }

            }
            else if (node is Spif)
            {
                Spif spif = (node as Spif);
                object r = spif.Exp.Eval(ast, State);
                if (!(r is bool))
                    throw new Exception("SPAZ spif must evaluate to bool SPAZ");
                bool rb = (bool)r;
                if (rb)
                {
                    if (node.Children.Count == 0)
                        throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS IF STATEMENT");
                    ret = Traverse(node.Children[0], false);
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
                    object r = spelzIf.Exp.Eval(ast, State);
                    if (!(r is bool))
                        throw new Exception("SPAZ spelzif must evaluate to bool SPAZ");
                    bool rb = (bool)r;
                    if (rb)
                    {
                        if (node.Children.Count == 0)
                            throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS SPELZIF STATEMENT");
                        ret = Traverse(node.Children[0], false);
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
                     ret = Traverse(node.Children[0], false);
                    
                }
            }
            else if (node is DoSpaz)
            {
                DoSpaz loop = (node as DoSpaz);
                object r = loop.Exp.Eval(ast, State);
                if (!(r is bool))
                    throw new Exception("SPAZ dospaz must evaluate to bool SPAZ");
                bool rb = (bool)r;
                if (rb)
                {
                    if (node.Children.Count == 0)
                        throw new Exception("MASSIVE SPAZ DOESNT HAVE ANYTHING IN HIS DOSPAZ STATEMENT");
                    ret = Traverse(node.Children[0], false); 
                }
                else
                    conCompleted = false;
            }


            //Regular execution
            Node next = GetNextChild(node);
            if (next != null && !swordOfKhali)
                ret = Traverse(next, conCompleted);

            return ret;
        }

        private Node GetSpazoutNext(Node n)
        {
            Node p = n;
            while (!(p is DoSpaz))
                p = p.Parent;
            
            spazoutInitiated = false;
            return GetNextChild(p);
        }

        private Node GetNextChild(Node n)
        {
            if (n == null || n.Parent == null)
                return null;

            if (spazoutInitiated)
                return GetSpazoutNext(n);


            for (int i = 0; i < n.Parent.Children.Count - 1; i++)
                if(n.Id == n.Parent.Children[i].Id)
                    return n.Parent.Children[i + 1];

            //If you are here you are looking at the last guy in that branch
            if (n.Parent is DoSpaz)
                return n.Parent;

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
