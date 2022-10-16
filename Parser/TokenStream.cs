namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Lexer;






public class TokenStream
{
    Stack<Token> Buffer { get; init; }
    bool _skipNewline = true;

    public TokenStream(string code)
    {
        Buffer = Lexer.Lex(code);
    }



    public Token Advance()
    {
        if (Buffer.TryPop(out Token current)) return current;

        return TokenType.EndOfText;
    }


    public bool CheckToken(params TokenType[] types)
    {
        if (_skipNewline)
        {
            while (Peek().Type == TokenType.NewLine)
            {
                Advance();
            }
        }
        foreach (var t in types)
        {
            if (Peek().Type == t) return true;
        }

        return false;
    }

    public Token Peek()
    {
        if (Buffer.TryPeek(out Token current))
        {
            return current;
        }
        return TokenType.EndOfText;
    }
    public bool Empty() => Buffer.Count <= 0;





    public void Expect(out Token matchedToken, params TokenType[] types)
    {
        if (!Match(out matchedToken, types))
        {
            var tokenTypesString = string.Join(", ", types);
            var message = $"Expected {tokenTypesString} but got {Peek().Type}";
            throw new Exception(message);
        }
    }

    public void ExpectNoSkip(out Token matchedToken, params TokenType[] types)
    {
        _skipNewline = false;
        Expect(out matchedToken, types);
        _skipNewline = true;
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










