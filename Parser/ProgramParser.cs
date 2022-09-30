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

        while(!CheckToken(TokenType.End) || CheckToken(TokenType.EndOfText))
        {
            Match(out Token matchedToken, TokenType.If, TokenType.While, TokenType.NewLine);
            var statement = matchedToken.Type switch
            {
                TokenType.If => IfBlockStatement(),
                TokenType.While => WhileStatement(),
                TokenType.NewLine => new EmptyStatement(),
                _ => ExpressionStatement()
            };

            statements.Add(statement);
        }

        if(!Match(TokenType.End, TokenType.EndOfText))
        {
            throw new Exception("A block must be closed with 'END' or be placed at the end of the code");
        }

        return new BlockStatement(statements);
    }





    IStatement ExpressionStatement()
    {
        var exp = new ExpressionStatement(Expression());
        Expect(TokenType.NewLine);
        
        return exp;
    }




    IStatement IfBlockStatement()
    {
        var expression = Conditional();
        var body = BlockList();

        if (Match(TokenType.ElseIf))
        {
            var elseIfStatement = IfBlockStatement();
            return new IfStatement(expression, body, elseIfStatement);
        }

        else if (Match(TokenType.Else))
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
        Expect(TokenType.Lparen, "Expected a parenthesis expression");
        var expression = Parenthesis();
        Expect(TokenType.Then, "Expected 'then' ");
        return expression;
    }
}

