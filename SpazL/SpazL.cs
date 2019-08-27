using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    public class SpazL
    {
        public static string Run(string file)
        {
            //Step 1. 
            Lexer l = new Lexer();
            var list = l.Tokenize(file);

            //Step 2.
            Parser p = new Parser();
            AST ast = p.Parse(list);

            //Step 3.
            Squirrel sq = new Squirrel(ast, TraverseMode.Interpret);
            sq.Traverse();
            return sq.GetTrace();
        }
    }
}
