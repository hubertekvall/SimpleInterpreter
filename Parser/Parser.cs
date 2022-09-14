using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimpleInterpreter;






public class Parser
{

    Token currentToken = new Token(TokenType.EndOfText);
    Token previousToken = new Token(TokenType.EndOfText);
    Lexer lexer;


    public Token Advance()
    {
        previousToken = currentToken;
        currentToken = lexer.GetNextToken();
        return previousToken;
    }

    public Token RetrieveToken() => previousToken;
    public Token Peek() => currentToken;
    public bool CheckToken(TokenType type) => Peek().type == type;


    public bool Expect(TokenType type, string message = "")
    {
        if (Match(type)) return true;

        throw new Exception(message);
    }



    public bool Match(params TokenType[] types)
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

    public (bool success, List<Token> matches) MatchPattern(params TokenType[] types)
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



    public abstract IStatement ParseCode(string text);
}




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













