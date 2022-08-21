using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace SimpleInterpreter
{
    public struct Parser
    {

        Token currentToken = new Token(TokenType.EndOfText);
        Token previousToken = new Token(TokenType.EndOfText);
        Lexer lexer;


        Token Advance()
        {
            previousToken = currentToken;
            currentToken = lexer.GetNextToken();

            return previousToken;
        }

        Token RetrieveToken()
        {
            return previousToken;
        }


        Token Peek()
        {

            return currentToken;
        }


        bool CheckToken(TokenType type)
        {
            return Peek().type == type;
        }





        bool Match(params TokenType[] types)
        {
            foreach (TokenType t in types)
            {
                if (CheckToken(t))
                {
                    Advance();

                    return true;
                }
            }
            return false;
        }

        (bool success, List<Token> matches) MatchPattern(params TokenType[] types)
        {
            var cachedLexer = lexer;
            var matches = new List<Token>(types.Length);


            foreach (TokenType t in types)
            {
                if (CheckToken(t))
                {
                    matches.Add(Advance());
                }
                else
                {
                    lexer = cachedLexer;
                    return (false, new List<Token>());
                }

            }
            return (true, matches);
        }






        public Parser(string text)
        {
            lexer = new Lexer(text);
            Advance();
        }



        public static TreeNode ParseCode(string text)
        {
            var parser = new Parser(text);

            return parser.Program();
        }






        List<TreeNode> Program()
        {
            return StatementList();
        }


        List<TreeNode> StatementList()
        {
            List<TreeNode> statements = new();

            while (Peek().type != TokenType.EndOfText)
            {
                statements.Add(Statement());
            }

            return statements;
        }

        TreeNode Statement()
        {

            if (Match(TokenType.If))
            {
                return ConditionalStatement();
            }

            else return Expression();
        }



        TreeNode ConditionalStatement()
        {
            return new Empty();
        }






        TreeNode Expression()
        {
            return Equality();
        }


        TreeNode Equality()
        {
            TreeNode rootNode = Relational();

            while (Match(TokenType.NotEquals, TokenType.Equals))
            {
                Token operand = RetrieveToken();
                TreeNode rightNode = Relational();
                return new BinaryOperator(operand, rootNode, rightNode);
            }
            return rootNode;
        }

        TreeNode Relational()
        {
            var rootNode = Term();

            while (Match(TokenType.GreaterOrEquals, TokenType.LesserOrEquals, TokenType.GreaterThan, TokenType.LesserThan))
            {
                var operand = RetrieveToken();
                var rightNode = Term();
                return new BinaryOperator(operand, rootNode, rightNode);
            }

            return rootNode;
        }

        TreeNode Term()
        {
            var rootNode = Factor();

            while (Match(TokenType.Add, TokenType.Subtract))
            {
                var operand = RetrieveToken();
                var rightNode = Factor();
                return new BinaryOperator(operand, rootNode, rightNode);
            }

            return rootNode;
        }

        TreeNode Factor()
        {
            var root = Unary();

            while (Match(TokenType.Divide, TokenType.Multiply))
            {
                var op = RetrieveToken();
                var right = Unary();
                root = new BinaryOperator(op, root, right);
            }

            return root;
        }

        TreeNode Unary()
        {
            if (Match(TokenType.Subtract, TokenType.Not))
            {
                var operand = RetrieveToken();
                return new UnaryOperator(operand, Primary());
            }

            else return Primary();
        }


        TreeNode Primary()
        {
            return Advance().type switch
            {
                TokenType.Number => Literal.Number(RetrieveToken()),
                TokenType.StringLiteral => Literal.String(RetrieveToken()),
                TokenType.Identifier => new LoadVariable(RetrieveToken().content),
                TokenType.Lparen => Parenthesis(),
                _ => throw new Exception("Expected an expression")
            };
        }



        TreeNode Parenthesis()
        {
            var expression = Expression();
            if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
            return expression;
        }




    }
}
