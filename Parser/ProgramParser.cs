using System;
namespace SimpleInterpreter;


public class ProgramParser : ExpressionParser
{

    public ProgramParser(string text) : base(text) { }



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
        if (Match(TokenType.If)) return IfElseStatement();
        else if (Match(TokenType.While)) return WhileStatement();
        else if (Match(TokenType.For)) return ForStatement();


        else return ExpStatement();
    }


    IStatement IfElseStatement()
    {
        var expression = ConditionalExpression();
        var body = Block();

        if (Match(TokenType.ElseIf))
        {
            var elseIfStatement = IfElseStatement();
            return new IfStatement(expression, body, elseIfStatement);
        }

        else if (Match(TokenType.Else))
        {
            var elseStatement = Statement();
            return new IfStatement(expression, body, elseStatement);
        }


        Expect(TokenType.End, "If statements needs to be closed with an 'END'");

        return new IfStatement(expression, body);
    }


    IStatement WhileStatement()
    {
        var expression = ConditionalExpression();
        
    }

    IStatement ForStatement()
    {

    }



    IStatement ExpStatement()
    {
        return new ExpressionStatement(Expression());
    }
}

