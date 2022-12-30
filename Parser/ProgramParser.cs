namespace SimpleInterpreter;


public record class ProgramParser(TokenStream Tokens) : ExpressionParser(Tokens)
{
    public static IStatement Parse(string code)
    {
        ProgramParser parser = new ProgramParser(new TokenStream(code));
        return parser.Program();
    }

    public IStatement Program() => new BlockTree { Statements = StatementList() };

    IStatement Block()
    {
        IStatement statements = new BlockTree { Statements = StatementList() };
        Tokens.Expect(TokenType.End);
        return statements;
    }




    List<IStatement> StatementList()
    {
        List<IStatement> statementList = new();
        while (!Tokens.CheckToken(TokenType.End, TokenType.EndOfText))
        {
            statementList.Add(Statement());
        }
        return statementList;
    }


    IStatement Statement()
    {
        Tokens.Match(out Token match, TokenType.While, TokenType.If);
        return match.Type switch
        {
            TokenType.If => ConditionalStatement(),
            TokenType.While => WhileStatement(),
            _ => ExpressionStatement(),
        };
    }





    IStatement ConditionalBlock()
    {
        Tokens.Expect(TokenType.Then);
        return Block();
    }


    IExpression ConditionalExpression()
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
                TokenType.ElseIf => new ConditionalStatementTree { Conditional = expression, MainBranch = body, ElseBranch = ConditionalStatement() },
                TokenType.Else => new ConditionalStatementTree { Conditional = expression, MainBranch = body, ElseBranch = ConditionalBlock() },
                _ => throw new Exception("Fatal error, should not occur")
            };
        }

        else
        {
            return new ConditionalStatementTree { Conditional = expression, MainBranch = body, ElseBranch = Empty.Statement };
        }
    }



    IStatement WhileStatement() => new WhileStatementTree { Conditional = ConditionalExpression(), Body = ConditionalBlock() };

    IStatement ExpressionStatement()
    {
        Tokens.SkipNewlines = false;
        IStatement expressionStatement = new ExpressionStatement { Expression = Expression() };
        Tokens.Expect(TokenType.NewLine, TokenType.EndOfText);
        Tokens.SkipNewlines = true;

        return expressionStatement;
    }





}

