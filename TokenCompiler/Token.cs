using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler
{
    public class Token
    {
        public int LineNumber { get; set; }
        public int InlinePosition { get; set; }
        public string TokenValue { get; set; }
        public TokenType TokenType { get; set; }

        public int Level { get; set; }
        public Token Partner { get; set; }


        public Token(int lineNumber, int inlinePosition, String tokenValue, TokenType tokenType, int level, Token partner)
        {
            this.LineNumber = lineNumber;
            this.InlinePosition = inlinePosition;
            this.TokenValue = tokenValue;
            this.TokenType = tokenType;
            this.Level = level;
            this.Partner = partner;
        }

        public override string ToString()
        {
            return "[" + this.LineNumber + "](" + this.InlinePosition + ")-" + this.TokenType + "--" + this.TokenValue + "-{" + this.Level + "}<" + this.Partner + ">";
        }

    }
}
