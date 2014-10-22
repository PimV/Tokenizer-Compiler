using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler
{
    class Program
    {
        static void Main(string[] args)
        {

            Tokenizer t = new Tokenizer();
            t.tokenize();
            t.printPartnerStack();
            t.printTokenList();

            Console.WriteLine(t.level);



            Console.ReadLine();

        }
    }
}
