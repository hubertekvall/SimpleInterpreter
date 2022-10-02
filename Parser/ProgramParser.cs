namespace SimpleInterpreter;


public class ProgramParser : ExpressionParser
{

    public ProgramParser(string text) : base(text) { }



    public IStatement Program()
    {
        return BlockList();

    }




    IStatement BlockList()
    {
        List<IStatement> statements = new();

        while (!CheckToken(TokenType.End))
        {

            switch (Peek().Type)
            {
                case TokenType.If:
                    Advance();
                    statements.Add(IfBlockStatement());
                    break;
                case TokenType.While:
                    Advance();
                    statements.Add(WhileStatement());
                    break;
                case TokenType.PrintStatement:
                    Advance();
                    var expression = Expression();
                    Terminal();
                    statements.Add(new PrintStatement(expression));
                    break;

                case TokenType.NewLine:
                    Advance();
                    break;
                default:
                    statements.Add(ExpressionStatement());
                    break;

            }
        }

        if (!Match(out _, TokenType.End, TokenType.EndOfText))
        {
            throw new Exception("A block must be closed with 'END' or be placed at the end of the code");
        }

        return new BlockStatement(statements);
    }





    IStatement ExpressionStatement()
    {
        var expstmnt = new ExpressionStatement(Expression());
        Terminal();
        return expstmnt;
    }


    void Terminal()
    {
        if (!Match(out _, TokenType.NewLine, TokenType.End)) throw new Exception();
    }



    IStatement IfBlockStatement()
    {
        var expression = Conditional();
        var body = BlockList();
        SkipNewlines();

        if (Match(out _, TokenType.ElseIf))
        {
            var elseIfStatement = IfBlockStatement();
            return new IfStatement(expression, body, elseIfStatement);
        }

        else if (Match(out _, TokenType.Else))
        {
            var elseStatement = BlockList();
            return new IfStatement(expression, body, elseStatement);
        }


        return new IfStatement(expression, body, null);
    }



    IStatement WhileStatement()
    {
        var expression = Conditional();
        var body = BlockList();

        return new WhileStatement(expression, body);
    }








    public IExpression Conditional()
    {
        Expect(TokenType.Lparen, out _, "Expected a parenthesis expression");
        var expression = Parenthesis();
        Expect(TokenType.Then, out _, "Expected 'then' ");
        return expression;
    }
}

