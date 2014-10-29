using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler.Compiler.Actions
{
    public class Assignment : CompilerAction
    {
        public Token LValue;
        public List<Token> RValue { get; set; }
        public List<Token> calculatedRValue { get; set; }

        public Assignment(List<Token> tokens)
        {
            this.Tokens = tokens;
            this.RValue = new List<Token>();

            this.LValue = tokens[0];

            //Start checking from '=' character
            for (int i = 2; i < tokens.Count; i++)
            {
                this.RValue.Add(tokens[i]);
            }
        }

        public int run()
        {
            if (this.calculatedRValue.Count == 1)
            {
                return Int32.Parse(calculatedRValue[0].TokenValue);
            }
            else
            {
                switch (calculatedRValue[1].TokenType)
                {
                    case TokenType.Plus:
                        return Int32.Parse(calculatedRValue[0].TokenValue) + Int32.Parse(calculatedRValue[2].TokenValue);
                    case TokenType.Minus:
                        return Int32.Parse(calculatedRValue[0].TokenValue) - Int32.Parse(calculatedRValue[2].TokenValue);
                }
            }
            return -1;
        }

        public override string ToString()
        {
            String rValueString = "";
            foreach (Token t in this.RValue)
            {
                rValueString += t.TokenValue + " ";
            }
            return "Assignment \t LValue: " + LValue.TokenValue + "\t RValue: " + rValueString;
        }
    }
}
