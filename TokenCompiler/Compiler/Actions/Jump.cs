using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler.Compiler.Actions
{
    public class Jump : CompilerAction
    {
        public LinkedListNode<CompilerAction> jumpLoc { get; set; }

        public Jump(LinkedListNode<CompilerAction> jumpLoc)
        {
            this.jumpLoc = jumpLoc;
        }

        public LinkedListNode<CompilerAction> jump()
        {
            return jumpLoc;
        }

        public override string ToString()
        {
            return "Jump \t Location: [" + jumpLoc.ToString() + "]";
        }

    }
}
