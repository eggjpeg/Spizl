using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    enum TokenType
    {
        Type,
        VarName,
        Op,
        Const,
        Eol,
        Command,
        Dot
    }
    enum VarType
    {
        Int, 
        Str,
        Lint,
        Lstr,
        Void
    }
    enum CommandType
    {
        Spif,
        Sprint,
        Spazout,
        Spazdun,
        Spelz,
        DoSpaz,
        Spelzif
    }
    enum OpType
    {
        Plus,
        Minus,
        Equal,
        Multiply,
        Divide,
        Mod,
        Morethan,
        Lessthan,
        MoreThanEq,
        LessThanEq,
        NotEq,
        Not,
        Oparen,
        Cparen,
        Comma,
        Obrack,
        Cbrack,
        Squote,
        Colon
    }
    class Token
    {
        public TokenType Type;
        public object SubType;
        public string Value;

        public Token(TokenType type)
        {
            this.Type = type;

        }
        public Token(TokenType type, string value)
        {
            Value = value;
            Type = type;
        }

        public Token(TokenType type, object subtype)
        {
            Type = type;
            SubType = subtype;
        }

        public override string ToString()
        {
            return (Type + " " + SubType + " " + Value);
        }

        public static string ToString(List<Token> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var token in list)
                sb.Append(token.ToString());
            return sb.ToString();
        }
    }
}