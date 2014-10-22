using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler
{
    public enum TokenType
    {
        //Keywords
        If,
        Else,
        While,
        Do,

        //Characters
        Plus,
        Minus,
        OpenParenth,
        CloseParenth,
        LBracket,
        RBracket,
        NotCompare,
        Compare,
        Equals,
        Semicolon,

        //Other
        Identifier,
        Number,
        Whitespace,
        Undetermined
    }
}
