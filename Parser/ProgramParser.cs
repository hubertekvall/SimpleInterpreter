namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;
using SimpleInterpreter.Lexer;

public record class ProgramParser(TokenStream Tokens) : ExpressionParser(Tokens)
{
    public static IStatement Parse(string code)
    {
        ProgramParser parser = new ProgramParser(new TokenStream(code));
        return parser.Program();
    }

    public IStatement Program() => new BlockStatement(StatementList());

    IStatement Block()
    {
        IStatement statements = new BlockStatement(StatementList());
        Tokens.Expect(TokenType.End);
        return statements;
    }


    IStatement ConditionalBlock()
    {
        Tokens.Expect(TokenType.Then);
        return Block();
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
        Tokens.Match(out Token match, TokenType.While, TokenType.If);
        return match.Type switch
        {
            TokenType.If => ConditionalStatement(),
            TokenType.While => WhileStatement(),
            // TokenType.Print => PrintStatement(),
            _ => ExpressionStatement(),
        };
    }

    public IExpression ConditionalExpression()
    {
        Tokens.Expect(TokenType.Lparen);
        return Parenthesis();
    }


    IStatement ConditionalStatement()
    {
        var expression = ConditionalExpression();
        var body = ConditionalBlock();


        if (Tokens.Match(out Token match, TokenType.ElseIf, TokenType.Else))
        {
            return match.Type switch
            {
                TokenType.ElseIf => new ConditionalStatement(expression, body, ConditionalStatement()),
                TokenType.Else => new ConditionalStatement(expression, body, ConditionalBlock()),
                _ => throw new Exception("Fatal error, should not occur")
            };
        }

        else
        {
            return new ConditionalStatement(expression, body, null);
        }
    }

    IStatement WhileStatement() => new WhileStatement(ConditionalExpression(), ConditionalBlock());
    // IStatement PrintStatement() => new PrintStatement(Expression());

    IStatement ExpressionStatement()
    {
   


        Tokens.SkipNewlines = false;
        IStatement expressionStatement = new ExpressionStatement(Expression());
        Tokens.Expect(TokenType.NewLine, TokenType.EndOfText);
        Tokens.SkipNewlines = true;

        return expressionStatement;
    }





}

