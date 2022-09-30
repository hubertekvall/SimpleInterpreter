namespace SimpleInterpreter;






public class BaseParser
{

    List<Token> tokenBuffer;
    int offset = 0;

    public BaseParser(string source)
    {
        tokenBuffer = new Lexer(source).GetTokens();
        if (tokenBuffer.Count == 0) throw new Exception("Can't parse an empty token list");
    }




    public Token GetToken()
    {
        if (!Finished()) offset++;
        return tokenBuffer[offset-1];
    }


    public bool CheckToken(TokenType type) => Peek().Type == type;

    public Token Peek() =>  tokenBuffer[offset];
    public bool Finished() => offset == tokenBuffer.Count - 1;


    public bool Expect(TokenType type, string message = "") => Expect(type, out Token matchedToken, message);
    public bool Expect(TokenType type, out Token matchedToken, string message = "")
    {
        if (Match(out matchedToken, type)) return true;
        throw new Exception(message);
    }

    public bool Match(params TokenType[] types) => Match(out Token matchedToken, types);
    public bool Match(out Token match, params TokenType[] types)
    {
        match = new Token(TokenType.EndOfText);
        foreach (TokenType t in types)
        {
            if (CheckToken(t))
            {
                match = GetToken();
                return true;
            }
        }
        return false;
    }




}


















