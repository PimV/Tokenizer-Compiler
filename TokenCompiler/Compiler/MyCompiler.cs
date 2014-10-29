using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCompiler.Compiler.Actions;

namespace TokenCompiler.Compiler
{
    public class MyCompiler
    {
        public LinkedList<CompilerAction> Actions { get; set; }

        public MyCompiler()
        {
            Actions = new LinkedList<CompilerAction>();
        }

        public void runCompile(List<Token> tokenList)
        {
            if (tokenList.Count > 0)
            {
                List<List<Token>> parts = partitionize(tokenList);
                foreach (List<Token> part in parts)
                {
                    processSubTokenList(part);
                }
            }
        }

        public void processSubTokenList(List<Token> subTokenList)
        {
            switch (subTokenList[0].TokenType)
            {
                case TokenType.Identifier:
                    createAssignment(subTokenList);
                    break;
                case TokenType.While:
                    createWhile(subTokenList);
                    break;
                case TokenType.If:
                    createIf(subTokenList);
                    break;
            }
        }

        public void createAssignment(List<Token> subTokenList)
        {
            subTokenList.RemoveAt(subTokenList.Count - 1);
            Actions.AddLast(new Assignment(subTokenList));
        }

        public void createWhile(List<Token> subTokenList)
        {
            DoNothing nothingStart = new DoNothing();
            Actions.AddLast(nothingStart);
            LinkedListNode<CompilerAction> nothingStartNode = Actions.Last;

            Condition condition = new Condition(createCondition(subTokenList));
            Actions.AddLast(condition);

            ConditionalJump condJump = new ConditionalJump();
            Actions.AddLast(condJump);

            DoNothing nothingTrue = new DoNothing();
            Actions.AddLast(nothingTrue);
            condJump.trueLoc = Actions.Last;

            List<List<Token>> body = processBody(subTokenList);
            foreach (List<Token> bodyPart in body)
            {
                processSubTokenList(bodyPart);
            }
            Actions.AddLast(new Jump(nothingStartNode));

            DoNothing nothingFalse = new DoNothing();
            Actions.AddLast(nothingFalse);
            condJump.falseLoc = Actions.Last;

        }

        public void createIf(List<Token> subTokenList)
        {
            //DoNothing nothingStart = new DoNothing();
            //actions.AddLast(nothingStart);
            //LinkedListNode<CompilerAction> nothingStartNode = actions.Last;

            Condition condition = new Condition(createCondition(subTokenList));
            Actions.AddLast(condition);

            ConditionalJump condJump = new ConditionalJump();
            Actions.AddLast(condJump);

            DoNothing nothingTrue = new DoNothing();
            Actions.AddLast(nothingTrue);
            condJump.trueLoc = Actions.Last;

            List<List<Token>> body = processBody(subTokenList);
            foreach (List<Token> bodyPart in body)
            {
                processSubTokenList(bodyPart);
            }

            DoNothing nothingFalse = new DoNothing();
            Actions.AddLast(nothingFalse);
            condJump.falseLoc = Actions.Last;
        }

        public List<Token> createCondition(List<Token> subTokenList)
        {
            List<Token> returnValue = new List<Token>();
            bool condition = false;

            foreach (Token t in subTokenList)
            {
                switch (t.TokenType)
                {
                    case TokenType.OpenParenth:
                        condition = true;
                        break;
                    case TokenType.CloseParenth:
                        return returnValue;
                }
                if (condition && t.TokenType != TokenType.OpenParenth)
                {
                    returnValue.Add(t);
                }
            }
            return returnValue;
        }

        public List<List<Token>> processBody(List<Token> subTokenList)
        {
            List<Token> bodyParts = new List<Token>();
            bool open = false;

            foreach (Token t in subTokenList)
            {
                if (t.TokenType == TokenType.LBracket)
                {
                    open = true;
                }

                if (open && t.TokenType != TokenType.LBracket)
                {
                    if (t.TokenType == TokenType.RBracket)
                    {
                        break;
                    }
                    bodyParts.Add(t);
                }
            }

            return partitionize(bodyParts);
        }


        public List<List<Token>> partitionize(List<Token> tokenList)
        {
            List<List<Token>> subTokenListList = new List<List<Token>>();

            List<Token> subTokenList = new List<Token>();

            foreach (Token t in tokenList)
            {
                subTokenList.Add(t);
                if ((t.TokenType == TokenType.RBracket) || (t.TokenType == TokenType.Semicolon && t.Level == subTokenList[0].Level))
                {
                    subTokenListList.Add(subTokenList);
                    subTokenList = new List<Token>();
                }
            }
            return subTokenListList;
        }


        public void printActionList(LinkedList<CompilerAction> actions)
        {
            foreach (CompilerAction ca in actions)
            {
                Console.WriteLine(ca.ToString());
            }
        }

    }
}
