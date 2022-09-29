namespace SimpleInterpreter;


public class ProgramParser : ExpressionParser
{

    public ProgramParser(string text) : base(text) { }



    IStatement Program()
    {
        return BlockList();
    }




    IStatement BlockList()
    {
        List<IStatement> statements = new();

        while (!CheckToken(TokenType.End) || !CheckToken(TokenType.EndOfText))
        {
            statements.Add(Statement());
        }
        return new BlockStatement(statements);
    }


    IStatement Block()
    {
        var list = BlockList();
        Expect(TokenType.End);

        return list;
    }

    IStatement Statement()
    {
        if (Match(TokenType.If)) return IfBlockStatement();
        else if (Match(TokenType.While)) return WhileStatement();
        else return new ExpressionStatement(Expression());
    }







    IStatement IfBlockStatement()
    {
        var expression = Conditional();
        var body = Block();

        if (Match(TokenType.ElseIf))
        {
            var elseIfStatement = IfBlockStatement();
            return new IfStatement(expression, body, elseIfStatement);
        }

        else if (Match(TokenType.Else))
        {
            var elseStatement = Statement();
            return new IfStatement(expression, body, elseStatement);
        }


        return new IfStatement(expression, body, null);
    }



    IStatement WhileStatement()
    {
        var expression = Conditional();
        var body = Block();

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

