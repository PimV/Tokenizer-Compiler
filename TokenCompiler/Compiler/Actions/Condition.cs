using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler.Compiler.Actions
{
    public class Condition : CompilerAction
    {
        public Token LValue { get; set; }
        public Token condition { get; set; }
        public Token RValue { get; set; }

        public Condition(List<Token> tokenList)
        {
            this.Tokens = tokenList;

            this.LValue = Tokens[0];
            this.condition = Tokens[1];
            this.RValue = Tokens[2];
        }

        public bool run()
        {
            switch (condition.TokenType)
            {
                case TokenType.Compare:
                    return LValue == RValue;
                case TokenType.NotCompare:
                    return LValue != RValue;
                default:
                    throw new ArgumentException("Not supported in our compiler");
            }
        }

        public override string ToString()
        {
            return "Condition \t LValue: " + LValue.TokenValue + "\t RValue: " + RValue.TokenValue + "\t Outcome: " + run();
        }
    }
}
