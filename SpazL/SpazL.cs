using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class SpazL
    {
        public static void Run(string file)
        {
            //Step 1. 
            Lexer l = new Lexer();
            var list = l.Tokenize(file);

            //Step 2.
            Parser p = new Parser();
            AST ast = p.Parse(list);

            //Step 3.
            Squirrel sq = new Squirrel(ast, TraverseMode.Interpret, true);
            sq.Traverse();
        }
    }
}
