namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Lexer;






public class TokenStream
{
    Stack<Token> Buffer { get; init; }


    public static TokenStream CreateFrom(string code)
    {
        
        var stack = new Stack<Token>(Lexer.Lex(code));
        stack.Reverse();

        TokenStream tStream = new TokenStream
        {
            Buffer = stack
        };

        return tStream;
    }



    public Token Advance()
    {
        if (Buffer.TryPop(out Token current))
        {
            return current;
        }

        return TokenType.End;
    }


    public bool CheckToken(TokenType type) => Peek().Type == type;
    public Token Peek()
    {
        if (Buffer.TryPeek(out Token current))
        {
            return current;
        }


        return TokenType.End;
    }
    public bool Empty() => Buffer.Count <= 0;



    public void SkipNewlines()
    {
        while (Peek().Type == TokenType.NewLine) Advance();
    }


    public bool Expect(TokenType type, out Token matchedToken, string message = "")
    {
        if (Match(out matchedToken, type)) return true;
        throw new Exception(message);
    }

    public bool Match(out Token match, params TokenType[] types)
    {
        match = new Token(TokenType.EndOfText);
        foreach (TokenType t in types)
        {
            if (CheckToken(t))
            {
                match = Advance();
                return true;
            }
        }
        return false;
    }
}










