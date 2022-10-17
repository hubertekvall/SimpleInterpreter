namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Lexer;






public class TokenStream
{
    Stack<Token> Buffer { get; init; }
    public bool SkipNewlines { get; set; }

    public TokenStream(string code)
    {
        Buffer = Lexer.Lex(code);
    }


    public bool Empty() => Buffer.Count == 0;

    public Token Advance()
    {
        if (Buffer.TryPop(out Token current)) return current;
        return TokenType.EndOfText;
    }


    public Token Peek()
    {
        if (Buffer.TryPeek(out Token currentToken))
        {
            if (SkipNewlines)
            {
                if (currentToken.Type == TokenType.NewLine)
                {
                    Advance();
                    currentToken = Peek();
                }
            }

            return currentToken;
        }


        return TokenType.EndOfText;
    }


    public bool CheckToken(params TokenType[] types)
    {
        foreach (var t in types)
        {
            if (Peek().Type == t) return true;
        }

        return false;
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




    public void Expect(params TokenType[] types)
    {

        if (!Match(out Token matchedToken, types))
        {
            var tokenTypesString = string.Join(", ", types);
            var message = $"Expected {tokenTypesString} but got {Peek().Type}";
            throw new Exception(message);
        }


    }




}










