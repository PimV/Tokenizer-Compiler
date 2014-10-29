using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCompiler.Compiler;

namespace TokenCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 100;


            Tokenizer t = new Tokenizer();
            t.createTokenList();
            t.printPartnerStack();
            t.printTokenList();

            MyCompiler compiler = new MyCompiler();
            compiler.runCompile(t.TokenList);
            compiler.printActionList(compiler.Actions);
            //Console.WriteLine(t.level);

            Console.ReadLine();

        }
    }
}
