using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpazL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("this is a test");
           // Test();
            DoParserTest();

            Console.ReadLine();
        }


        //        int j = 2
        //        int i = 2 + 3 - (5 * add(2 * 4, 3 - 3, mul(4 * 2, j * 3 * j)))
        //        sprint(i)
        //        spif i != 9
        //        sprint(i + 1).

        static void Test()
        {
            int j = 2;
            int i = 2 + 3 - (5 * Add(2 * 4, 3 - 3, Mul(4 * 2, j * 3 * j)));
            Console.WriteLine(i);
            if(i!=9)
                Console.WriteLine(i + 1);
        }


        static int Mul(int a, int b)
        {
            return a * b;
        }


        static int Add(int a, int b, int c)
        {
            return a + b + c;
        }
        static void DoExpTest()
        {
            List<Token> list = new List<Token>();
            list.Add(new Token(TokenType.Type, OpType.Oparen));
            list.Add(new Token(TokenType.Const,"2"));
            list.Add(new Token(TokenType.Type, OpType.Plus));
            list.Add(new Token(TokenType.Const, "3"));
            list.Add(new Token(TokenType.Type, OpType.Cparen));
            list.Add(new Token(TokenType.Type, OpType.Multiply));
            list.Add(new Token(TokenType.Const, "7"));

            Expression exp = new Expression(list,false);
            Console.WriteLine(exp.BuildTree().ToString());
        }

        static void DoParserTest()
        {
            //Step 1. 
            Lexer l = new Lexer();
            var list = l.Tokenize("Spazl/lists.spaz");

            //Step 2.
            Parser p = new Parser();
            AST ast = p.Parse(list);

            
            //Step 3.
            Squirrel sq = new Squirrel(ast,TraverseMode.Interpret);
            sq.Traverse();

        }
    }
}
