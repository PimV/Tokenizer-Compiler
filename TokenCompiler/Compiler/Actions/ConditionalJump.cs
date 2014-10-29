using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler.Compiler.Actions
{
    public class ConditionalJump : CompilerAction
    {
        public LinkedListNode<CompilerAction> trueLoc { get; set; }
        public LinkedListNode<CompilerAction> falseLoc { get; set; }

        public ConditionalJump() { }

        public LinkedListNode<CompilerAction> jump(bool condition)
        {
            if (condition)
            {
                return trueLoc;
            }
            else
            {
                return falseLoc;
            }

        }

        public override string ToString()
        {
            return "Conditional Jump \n \t\t TrueAction: " + trueLoc.Value.ToString() + "\n\t\t FalseAction: " + falseLoc.Value.ToString();
        }
    }
}
