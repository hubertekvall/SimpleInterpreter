public class StatementParser : ExpressionParser
{

    IStatement Program()
    {
        return Block();
    }


    IStatement Block()
    {
        List<IStatement> statements = new();

        while (!CheckToken(TokenType.End) || !CheckToken(TokenType.EndOfText))
        {
            statements.Add(Statement());
        }


        return new BlockStatement(statements);
    }

    IStatement Statement()
    {
        if (Match(TokenType.If))
        {
            return ConditionalStatement();
        }


        else return ExpStatement();
    }


    IStatement ConditionalStatement()
    {
        Expect(TokenType.Lparen, "Expression must be initiated by a left-parenthesis");
        var expression = Expression();
        Expect(TokenType.Rparen, "Expression must be closed by a right-parenthesis");
        Expect(TokenType.Then, "Expected 'then'");
        var body = Block();

        if (Match(TokenType.ElseIf))
        {
            var elseIfStatement = ConditionalStatement();
            return new IfStatement(expression, body, elseIfStatement);
        }

        else if (Match(TokenType.Else))
        {
            var elseStatement = Statement();
            return new IfStatement(expression, body, elseStatement);
        }


        Expect(TokenType.End, "If statements need to be close with a 'END'");

        return new IfStatement(expression, body, null);
    }


    IStatement ExpStatement()
    {
        return new ExpressionStatement(Expression());
    }
}

