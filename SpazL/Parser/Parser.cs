using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Parser
    {
        public AST Parse(List<Token> tList)
        {
            AST ast = new AST();
            Node parent = ast;
            var lines = GetLines(tList);
            Print(lines);
            foreach (var item in lines)
            {
               parent = ProcessLine(item, parent);    
            }
            ast.PrintPretty(" ", true);
            return ast;
        }

        public void Print(List<List<Token>> list)
        {
            foreach (var item in list)
            {
                foreach (var item2 in item)
                {
                    Console.Write(item2.ToString());
                    
                }
                Console.WriteLine();
            }
        }

        public List<List<Token>> GetLines(List<Token> list)
        {
            var r = new List<List<Token>>();
            var l = new List<Token>();
            foreach (var item in list)
            {
                if(item.Type == TokenType.Eol)
                {
                    r.Add(l);
                    l = new List<Token>();
                    continue;
                }                   
                l.Add(item);

            }
            return r;
        }

        private bool HasOp(List<Token> line, OpType op)
        {
            foreach (var item in line)
                if (item.Type == TokenType.Op && (OpType)item.SubType == op)
                    return true;
            return false;
        }
        public Node ProcessLine(List<Token> line, Node parent)
        {
            
            int kd = KillDots(line);
            Node newParent = parent;

            if(line.Count > 0)
            {
                if (HasOp(line, OpType.Colon))
                {
                    FunctionDef f = new FunctionDef(line);
                    parent.Add(f);
                    f.Parent = parent;
                    newParent = f;
                }
                else if (line[0].Type == TokenType.Type)
                {
                    Declaration d;
                    if (line.Count == 2)
                        d = new Declaration((VarType)line[0].SubType, line[1].Value, null);
                    else
                        d = new Declaration((VarType)line[0].SubType, line[1].Value, line.GetRange(3, line.Count - 3));

                    parent.Add(d);
                    d.Parent = parent;
                }
                else if(line[0].Type == TokenType.Command && (CommandType)line[0].SubType == CommandType.DoSpaz)
                {
                    DoSpaz l = new DoSpaz(line.GetRange(1, line.Count - 1));
                    parent.Add(l);
                    l.Parent = parent;
                    newParent = l;
                }
                else if (line[0].Type == TokenType.Command && (CommandType)line[0].SubType == CommandType.Spif)
                {
                    Spif s = new Spif(line.GetRange(1, line.Count - 1));
                    parent.Add(s);
                    s.Parent = parent;
                    newParent = s;
                }
                else if (line[0].Type == TokenType.Command && (CommandType)line[0].SubType == CommandType.Spelz)
                {
                    Spelz s = new Spelz();
                    parent.Add(s);
                    s.Parent = parent;
                    newParent = s;
                }
                else if (line[0].Type == TokenType.Command && (CommandType)line[0].SubType == CommandType.Spelzif)
                {
                    Spelzif s = new Spelzif(line.GetRange(1, line.Count - 1));
                    parent.Add(s);
                    s.Parent = parent;
                    newParent = s;
                }
                else if (HasOp(line, OpType.Oparen)) 
                {
                    FunctionCall f = new FunctionCall(line);
                    parent.Add(f);
                    f.Parent = parent;
                }
                else if(HasOp(line, OpType.Equal))
                {
                    Assignment a = new Assignment(line);
                    parent.Add(a);
                    a.Parent = parent;
                }
                else
                    throw new Exception("Unknown line. spaz.");
            }


            for (int i = 0; i < kd; i++)
                newParent = newParent.Parent;

            return newParent;
        }

        public int KillDots(List<Token> list)
        {
            if (list.Count == 0)
                return 0;
            int i = 0;
            while (list[list.Count-1].Type == TokenType.Dot)
            {
                i++;
                list.RemoveAt(list.Count - 1);
            }
            return i;
        }

    }
}
