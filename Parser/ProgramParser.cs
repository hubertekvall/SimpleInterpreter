namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;
using SimpleInterpreter.Lexer;
using SimpleInterpreter.Runtime.Operators;

public class ProgramParser : ExpressionParser
{

    public static IStatement Parse(string code)
    {
        ProgramParser parser = new ProgramParser
        {
            Tokens = TokenStream.CreateFrom(code)
        };

        return parser.Program();
    }


    public IStatement Program()
    {
        return BlockList();
    }




    IStatement BlockList()
    {
        List<IStatement> statements = new();

        while (!Tokens.CheckToken(TokenType.End))
        {

            switch (Tokens.Peek().Type)
            {
                case TokenType.If:
                    Tokens.Advance();
                    statements.Add(ConditionalStatement());
                    break;
                case TokenType.While:
                    Tokens.Advance();
                    statements.Add(WhileStatement());
                    break;
                case TokenType.PrintStatement:
                    Tokens.Advance();
                    var expression = Expression();
                    Terminal();
                    statements.Add(new PrintStatement(expression));
                    break;

                case TokenType.NewLine:
                    Tokens.Advance();
                    break;
                default:
                    statements.Add(ExpressionStatement());
                    break;

            }
        }

        if (!Tokens.Match(out _, TokenType.End, TokenType.EndOfText))
        {
            throw new Exception("A block must be closed with 'END' or be placed at the end of the code");
        }

        return new BlockStatement(statements);
    }








    IStatement ConditionalStatement()
    {
        var expression = ConditionalExpression();
        var body = BlockList();
        Tokens.SkipNewlines();

        if (Tokens.Match(out _, TokenType.ElseIf))
        {
            var elseIfStatement = ConditionalStatement();
            return new ConditionalStatement(expression, body, elseIfStatement);
        }

        else if (Tokens.Match(out _, TokenType.Else))
        {
            var elseStatement = BlockList();
            return new ConditionalStatement(expression, body, elseStatement);
        }


        return new ConditionalStatement(expression, body, null);
    }



    IStatement WhileStatement()
    {
        var expression = ConditionalExpression();
        var body = BlockList();

        return new WhileStatement(expression, body);
    }


    public IExpression ConditionalExpression()
    {
        Tokens.Expect(TokenType.Lparen, out _, "Expected a parenthesis expression");
        var expression = Parenthesis();
        Tokens.Expect(TokenType.Then, out _, "Expected 'then' ");
        return expression;
    }



    IStatement ExpressionStatement()
    {
        var statement = new ExpressionStatement(Expression());
        Terminal();
        return statement;
    }


    void Terminal()
    {
        if (!Tokens.Match(out _, TokenType.NewLine, TokenType.End)) throw new Exception();
    }

}

