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
            Tokens = new TokenStream(code)
        };

        return parser.Program();
    }



    public IStatement Program() => new BlockStatement(StatementList());




    IStatement Block()
    {
        IStatement statements = new BlockStatement(StatementList());
        Tokens.Expect(out _, TokenType.End);
        return statements;
    }




    List<IStatement> StatementList()
    {
        List<IStatement> stmntList = new();

        while (!Tokens.CheckToken(TokenType.End, TokenType.EndOfText))
        {
            stmntList.Add(Statement());
        }

        return stmntList;
    }


    IStatement Statement()
    {

        Tokens.Match(out Token match, TokenType.While, TokenType.If, TokenType.PrintStatement);
        var statement = match.Type switch
        {
            TokenType.If => ConditionalStatement(),
            TokenType.While => WhileStatement(),
            _ => ExpressionStatement()
        };

        return statement;
    }


    IStatement ConditionalBlock()
    {
        Tokens.Expect(out _, TokenType.Then);
        return Block();
    }


    IStatement ConditionalStatement()
    {
        var expression = ConditionalExpression();
        var body = ConditionalBlock();
   
        if (Tokens.Match(out _, TokenType.ElseIf))
        {
            var elseIfStatement = ConditionalStatement();
            return new ConditionalStatement(expression, body, elseIfStatement);
        }

        else if (Tokens.Match(out _, TokenType.Else))
        {
            var elseStatement = ConditionalBlock();
            return new ConditionalStatement(expression, body, elseStatement);
        }


        return new ConditionalStatement(expression, body, null);
    }



    IStatement WhileStatement() => new WhileStatement(ConditionalExpression(), ConditionalBlock());


    public IExpression ConditionalExpression()
    {
        Tokens.Expect(out _, TokenType.Lparen);
        return Parenthesis();
    }



    IStatement ExpressionStatement()
    {
        var statement = new ExpressionStatement(Expression());
        Tokens.ExpectNoSkip(out _, TokenType.NewLine, TokenType.End);
        return statement;
    }





}

