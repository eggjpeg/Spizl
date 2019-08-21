using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpazL
{
    class Lexer
    {
        public char[] OpList = { '*', '-', '+', '/', '%', '=', '>', '<','!','(',')','.',',','[',']'};
        public void PrintTokens(List<Token> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }

        }

        
        
        public List<Token> PostProcessOps(List<Token> list)
        {
            var nlist = new List<Token>();

            for (int i = 0; i < list.Count; i++)
            {
                bool isDefault = false;
                if (list.Count-1>i && list[i].SubType != null && list[i+1].SubType != null)
                {
                    if (list[i].Type == TokenType.Op && list[i + 1].Type == TokenType.Op && (OpType)list[i + 1].SubType == OpType.Equal)
                    {
                        switch ((OpType)list[i].SubType)
                        {
                            case OpType.Lessthan: nlist.Add(new Token(TokenType.Op, OpType.LessThanEq)); break;
                            case OpType.Morethan: nlist.Add(new Token(TokenType.Op, OpType.MoreThanEq)); break;
                            case OpType.Not: nlist.Add(new Token(TokenType.Op, OpType.NotEq)); break;
                            default: isDefault = true; break;
                        }

                        if (!isDefault)
                        {
                            i++;
                            continue;
                        }
                    }

                }
                nlist.Add(list[i]);
            }


            return nlist;
        }

        //a 's f f' s
        public static List<string> Split(string line, char seperator)
        {
            List<string> result = new List<string>();
            bool isInString = false;

            StringBuilder sb = new StringBuilder();
            foreach (char c in line)
            {
                if (c == '\'')
                {
                    isInString = !isInString;
                    if (!isInString)
                    {
                        result.Add("'" + sb.ToString() + "'");
                        sb.Clear();
                    }
                }
                else
                {
                    if(c == seperator && !isInString)
                    {
                        if (sb.Length > 0)
                        {
                            result.Add(sb.ToString());
                            sb.Clear();
                        }
                    }
                    else
                        sb.Append(c);
                }
            }

            if(sb.Length > 0)
                result.Add(sb.ToString());

            return result;

        }

        public List<Token> Tokenize(string spazfile)
        {
            var list = new List<Token>();
            using (StreamReader sr = new StreamReader(spazfile))
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    line = SpaceOut(line);
                    List<string> linear = Split(line,' ');

                    foreach (var item in linear)
                    {
                        if (item.Trim() == "")
                            continue;

                        var itml = item;
                        if(!item.StartsWith("'"))
                            itml = item.ToLower().Trim();

                        var token = Classify(itml);
                       
                        list.Add(token);
                    }
                    list.Add(new Token(TokenType.Eol));
                }
            }
            return PostProcessOps(list);
        }

        private bool IsOp(char c)
        {
            foreach(var op in OpList)
                if (op == c)
                    return true;
            return false;
        }

        //TODO: Fix when dealing with quotes
        private string SpaceOut(string line)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            { 
                if(IsOp(line[i]))
                    sb.Append(' ' + line[i].ToString() + ' ');
                else
                    sb.Append(line[i].ToString());
            }
            return sb.ToString();
        }

        private Token Classify(string item)
        {
            if(item.StartsWith("'"))
                return new Token(TokenType.Const, item);

            switch (item)
            {
                case "spif": return new Token(TokenType.Command, CommandType.Spif);
                case "spelz": return new Token(TokenType.Command, CommandType.Spelz);
                case "spelzif": return new Token(TokenType.Command, CommandType.Spelzif);
                case "spazout": return new Token(TokenType.Command, CommandType.Spazout);
                case "spazdun": return new Token(TokenType.Command, CommandType.Spazdun);
                case "dospaz": return new Token(TokenType.Command, CommandType.DoSpaz);
                case "sprint": return new Token(TokenType.Command, CommandType.Sprint);
                case "/": return new Token(TokenType.Op, OpType.Divide);
                case "*": return new Token(TokenType.Op, OpType.Multiply);
                case "+": return new Token(TokenType.Op, OpType.Plus);
                case "-": return new Token(TokenType.Op, OpType.Minus);
                case "!": return new Token(TokenType.Op, OpType.Not);
                case "=": return new Token(TokenType.Op, OpType.Equal);
                case "%": return new Token(TokenType.Op, OpType.Mod);
                case ">": return new Token(TokenType.Op, OpType.Morethan);
                case "<": return new Token(TokenType.Op, OpType.Lessthan);
                case "int": return new Token(TokenType.Type, VarType.Int);
                case "str": return new Token(TokenType.Type, VarType.Str);
                case "lint": return new Token(TokenType.Type, VarType.Lint);
                case "lstr": return new Token(TokenType.Type, VarType.Lstr);
                case "spaz": return new Token(TokenType.Type, VarType.Spaz);
                case "\n": return new Token(TokenType.Eol);
                case ".": return new Token(TokenType.Dot);
                case "(": return new Token(TokenType.Op, OpType.Oparen);
                case ")": return new Token(TokenType.Op, OpType.Cparen);
                case "[": return new Token(TokenType.Op, OpType.Obrack);
                case "]": return new Token(TokenType.Op, OpType.Cbrack);
                case ",": return new Token(TokenType.Op, OpType.Comma);
                case "'": return new Token(TokenType.Op, OpType.Squote);

                default: break;
            }

            if (Util.IsNumber(item))
                return new Token(TokenType.Const, item);
            else
                return new Token(TokenType.VarName, item);
        }
    }
}
