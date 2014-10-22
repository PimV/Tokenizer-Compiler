using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCompiler
{
    public class Tokenizer
    {
        public Dictionary<String, TokenType> Matchers_Keyword { get; set; }
        public Dictionary<String, TokenType> Matchers_Character { get; set; }
        public string[] lines_sample1 { get; set; }
        public string[] lines_sample2 { get; set; }
        public string[] lines_sample3 { get; set; }
        public Stack<Token> PartnerStack { get; set; }
        public List<Token> TokenList { get; set; }
        public List<Token> ErrorList { get; set; }

        public bool debug { get; set; }

        public int level { get; set; }


        public Tokenizer()
        {
            debug = false;
            init();
        }

        public void init()
        {
            lines_sample1 = System.IO.File.ReadAllLines("sample1.txt");
            lines_sample2 = System.IO.File.ReadAllLines("sample2.txt");
            lines_sample3 = System.IO.File.ReadAllLines("sample3.txt");

            this.PartnerStack = new Stack<Token>();
            this.TokenList = new List<Token>();
            this.ErrorList = new List<Token>();

            this.Matchers_Keyword = new Dictionary<String, TokenType>();
            this.Matchers_Keyword.Add("if", TokenType.If);
            this.Matchers_Keyword.Add("else", TokenType.Else);
            this.Matchers_Keyword.Add("while", TokenType.While);

            this.Matchers_Character = new Dictionary<string, TokenType>();
            this.Matchers_Character.Add(" ", TokenType.Whitespace);
            this.Matchers_Character.Add("=", TokenType.Equals);
            this.Matchers_Character.Add("==", TokenType.Compare);
            this.Matchers_Character.Add("!=", TokenType.NotCompare);
            this.Matchers_Character.Add("+", TokenType.Plus);
            this.Matchers_Character.Add("-", TokenType.Minus);
            this.Matchers_Character.Add("(", TokenType.OpenParenth);
            this.Matchers_Character.Add(")", TokenType.CloseParenth);
            this.Matchers_Character.Add("{", TokenType.LBracket);
            this.Matchers_Character.Add("}", TokenType.RBracket);

            this.Matchers_Character.Add(";", TokenType.Semicolon);

            this.level = 0;
        }

        public void tokenize()
        {
            init();
            for (int lineNumber = 0; lineNumber < lines_sample3.Length; lineNumber++)
            {
                string currentLine = lines_sample3[lineNumber];
                string currentToken = "";
                for (int position = 0; position < currentLine.Length; ++position)
                {
                    currentToken += currentLine[position];
                    if (isLetter(currentToken[0]))
                    {
                        position = processKeyword(currentLine, currentToken, lineNumber + 1, position);
                    }
                    else if (isDigit(currentToken[0]))
                    {
                        position = processNumber(currentLine, currentToken, lineNumber + 1, position);
                    }
                    else if (isWhitespace(currentToken[0]))
                    {
                        position = processWhitespace(currentLine, currentToken, lineNumber + 1, position);
                    }
                    else if (isSpecialChar(currentToken[0]))
                    {
                        position = processSpecialCharacter(currentLine, currentToken, lineNumber + 1, position);
                    }
                    currentToken = "";
                }
            }
            checkPartnerErrors();
        }

        #region Token Processing
        public int processKeyword(string currentLine, string currentToken, int lineNumber, int position)
        {
            int newPosition = position + 1;

            for (int i = position + 1; i < currentLine.Length; i++)
            {
                newPosition = i;
                if (isWhitespace(currentLine[i]) || isSpecialChar(currentLine[i]))
                {
                    newPosition = i - 1;
                    break;
                }
                currentToken += currentLine[i];
            }
            currentToken = currentToken.Trim();
            Token t = new Token(lineNumber, position, currentToken, TokenType.Identifier, this.level, null);
            if (this.Matchers_Keyword.Keys.Contains(currentToken))
            {
                t.TokenType = this.Matchers_Keyword[currentToken];
            }


            handlePartners(t);

            this.TokenList.Add(t);

            if (debug)
            {
                Console.WriteLine("New token (keyword/identifier): " + currentToken);
            }
            return newPosition;
        }

        public int processNumber(string currentLine, string currentToken, int lineNumber, int position)
        {
            int newPosition = position + 1;

            for (int i = position + 1; i < currentLine.Length; i++)
            {
                newPosition = i;
                if (isWhitespace(currentLine[i]) || !isDigit(currentLine[i]))
                {
                    newPosition = i - 1;
                    break;
                }
                currentToken += currentLine[i];
            }
            currentToken = currentToken.Trim();

            Token t = new Token(lineNumber, position, currentToken, TokenType.Number, this.level, null);

            this.TokenList.Add(t);

            if (debug)
            {
                Console.WriteLine("New token (number): " + currentToken);
            }
            return newPosition;
        }

        public int processWhitespace(string currentLine, string currentToken, int lineNumber, int position)
        {
            int newPosition = position + 1;

            for (int i = position + 1; i < currentLine.Length; i++)
            {
                newPosition = i;
                if (!isWhitespace(currentLine[i]))
                {
                    newPosition = i - 1;
                    break;
                }
                currentToken += currentLine[i];
            }
            if (debug)
            {
                Console.WriteLine("New token (whitespace): " + currentToken);
            }
            return newPosition;
        }


        public int processSpecialCharacter(string currentLine, string currentToken, int lineNumber, int position)
        {
            int newPosition = position + 1;

            for (int i = position + 1; i < currentLine.Length; i++)
            {
                newPosition = i;
                if (isSemicolon(currentLine[i]) || isWhitespace(currentLine[i]) || !isSpecialChar(currentLine[i]))
                {
                    newPosition = i - 1;
                    break;
                }
                currentToken += currentLine[i];
            }
            currentToken = currentToken.Trim();

            Token t = new Token(lineNumber, position, currentToken, TokenType.Undetermined, this.level, null);
            if (this.Matchers_Character.Keys.Contains(currentToken))
            {
                t.TokenType = this.Matchers_Character[currentToken];
            }

            handlePartners(t);

            this.TokenList.Add(t);


            if (debug)
            {
                Console.WriteLine("New token (special character): " + currentToken);
            }
            return newPosition;
        }
        #endregion

        #region Partner, Level and Error Handling
        public void handlePartners(Token t)
        {
            switch (t.TokenType)
            {
                case TokenType.If:
                    PartnerStack.Push(t);
                    break;
                case TokenType.Else:
                    if (PartnerStack.Count > 0 && PartnerStack.Peek().TokenType == TokenType.If)
                    {
                        Console.WriteLine("Adding partner to if");
                        t.Partner = PartnerStack.Peek();
                        PartnerStack.Pop().Partner = t;
                    }
                    else
                    {
                        this.ErrorList.Add(t);
                    }
                    break;
                case TokenType.Do:
                    PartnerStack.Push(t);
                    break;
                case TokenType.While:
                    if (PartnerStack.Count > 0 && PartnerStack.Peek().TokenType == TokenType.Do)
                    {
                        t.Partner = PartnerStack.Peek();
                        PartnerStack.Pop().Partner = t;
                    }
                    else
                    {
                        this.ErrorList.Add(t);
                    }
                    break;
                case TokenType.LBracket:
                    this.level++;
                    PartnerStack.Push(t);
                    break;
                case TokenType.RBracket:

                    if (PartnerStack.Count > 0 && PartnerStack.Peek() != null && PartnerStack.Peek().TokenType == TokenType.LBracket)
                    {
                        this.level--;
                        t.Partner = PartnerStack.Peek();
                        PartnerStack.Pop().Partner = t;
                    }
                    else
                    {
                        this.ErrorList.Add(t);
                    }
                    break;
                case TokenType.OpenParenth:
                    this.level++;
                    PartnerStack.Push(t);
                    break;
                case TokenType.CloseParenth:
                    if (PartnerStack.Count > 0 && PartnerStack.Peek() != null && PartnerStack.Peek().TokenType == TokenType.OpenParenth)
                    {
                        this.level--;
                        t.Partner = PartnerStack.Peek();
                        PartnerStack.Pop().Partner = t;
                    }
                    else
                    {
                        this.ErrorList.Add(t);
                    }
                    break;
            }
        }

        public void checkPartnerErrors()
        {
            if (this.level == 0 && PartnerStack.Count < 1 && ErrorList.Count < 1)
            {
                Console.WriteLine("No errors found");
            }
            else
            {
                Token current;
                while (PartnerStack.Count > 0)
                {
                    current = PartnerStack.Pop();
                    switch (current.TokenType)
                    {
                        case TokenType.Else:
                        case TokenType.Do:
                        case TokenType.LBracket:
                        case TokenType.RBracket:
                        case TokenType.OpenParenth:
                        case TokenType.CloseParenth:
                            Console.WriteLine("Errors occurred on token: " + current.TokenType + " [" + current.TokenValue + "]" + "(Line: " + current.LineNumber + ")");
                            break;
                    }
                }
                printErrorList();
            }

        }
        #endregion

        #region Checks
        public bool isSemicolon(char c)
        {
            return c == ';';
        }

        public bool isLetter(char c)
        {
            return Char.IsLetter(c);
        }

        public bool isDigit(char c)
        {
            return Char.IsDigit(c);
        }

        public bool isWhitespace(char c)
        {
            return Char.IsWhiteSpace(c);
        }

        public bool isSpecialChar(char c)
        {
            return !(Char.IsLetterOrDigit(c)) || Char.IsWhiteSpace(c);
        }
        #endregion

        #region Printing
        public void printPartnerStack()
        {
            foreach (Token t in this.PartnerStack)
            {
                Console.WriteLine(t.TokenType);
            }
        }


        public void printTokenList()
        {
            foreach (Token t in this.TokenList)
            {
                Console.WriteLine(t.TokenType + "[" + t.TokenValue + "]");
            }
        }

        public void printErrorList()
        {
            foreach (Token t in this.ErrorList)
            {
                Console.WriteLine("Errors occurred on token: " + t.TokenType + " [" + t.TokenValue + "]" + "(Line: " + t.LineNumber + ")");
            }
        }

        public void printSample1()
        {
            Console.WriteLine("##### Sample 1 #####");
            foreach (string s in lines_sample1)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("##### Sample 1 #####");
        }

        public void printSample2()
        {
            Console.WriteLine("##### Sample 2 #####");
            foreach (string s in lines_sample2)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("##### Sample 2 #####");
        }
        #endregion


    }
}
