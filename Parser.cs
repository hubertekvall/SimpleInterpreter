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



        public static List<IStatement> ParseCode(string text)
        {
            var parser = new Parser(text);

            return parser.Program();
        }






        List<IStatement> Program()
        {
            return StatementList();
        }


        List<IStatement> StatementList()
        {
            List<IStatement> statements = new();

            while (Peek().type != TokenType.EndOfText)
            {
                statements.Add(Statement());
            }

            return statements;
        }

        IStatement Statement()
        {
            if (Match(TokenType.If))
            {
                // return ConditionalStatement();
            }

            return ExpStatement();
        }



    



        IStatement ExpStatement()
        {
            return new ExpressionStatement(Expression());
        }



        IExpression Expression()
        {
            return Equality();
        }


        IExpression Equality()
        {
            IExpression rootNode = Relational();

            while (Match(TokenType.NotEquals, TokenType.Equals))
            {
                Token operand = RetrieveToken();
                IExpression rightNode = Relational();
                return new BinaryOperator(operand, rootNode, rightNode);
            }
            return rootNode;
        }

        IExpression Relational()
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

        IExpression Term()
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

        IExpression Factor()
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

        IExpression Unary()
        {
            if (Match(TokenType.Subtract, TokenType.Not))
            {
                var operand = RetrieveToken();
                return new UnaryOperator(operand, Primary());
            }

            else return Primary();
        }


        IExpression Primary()
        {
            return Advance().type switch
            {
                TokenType.Number => new Literal(double.Parse(RetrieveToken().content)),
                TokenType.StringLiteral => new Literal(RetrieveToken().content),
                TokenType.Identifier => new Load(RetrieveToken().content),
                TokenType.Lparen => Parenthesis(),
                _ => throw new Exception("Expected an expression")
            };
        }



        IExpression Parenthesis()
        {
            var expression = Expression();
            if (!Match(TokenType.Rparen)) throw new Exception("Unclosed parenthesis");
            return expression;
        }




    }
}
