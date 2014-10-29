using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler.Compiler.Actions
{
    public class DoNothing : CompilerAction
    {
        public override string ToString()
        {
            return "Do Nothing";
        }
    }
}
