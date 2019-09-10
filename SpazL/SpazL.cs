using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    public class SpazL
    {
        private string file;
        public SpazL(string file)
        {
            this.file = file;
        }

        public string Result;

        public void Run()
        {
            //Step 1. 
            Lexer l = new Lexer();
            var list = l.Tokenize(file);

            //Step 2.
            Parser p = new Parser();
            AST ast = p.Parse(list);

            //Step 3.
            Squirrel sq = new Squirrel(ast);
            sq.Traverse();
            this.Result = sq.GetTrace();
        }
    }
}
