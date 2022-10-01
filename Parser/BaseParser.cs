namespace SimpleInterpreter;






public class BaseParser
{
    Stack<Token> tokenBuffer;
   

    public BaseParser(string source)
    {
        tokenBuffer = new Stack<Token>(new Lexer(source).GetTokens().Reverse<Token>());
        if (tokenBuffer.Count == 0) throw new Exception("Can't parse an empty token list");
    }



    public Token Advance()
    {
        if (tokenBuffer.TryPop(out Token current))
        {
            return current;
        }

        return TokenType.End;
    }


    public bool CheckToken(TokenType type) => Peek().Type == type;
    public Token Peek()
    {
        if(tokenBuffer.TryPeek(out Token current))
        {
            return current;
        }
            
                
        return TokenType.End;
    }
    public bool Empty() => tokenBuffer.Count <= 0;

  

    public void SkipNewlines()
    {
        while(Peek().Type == TokenType.NewLine) Advance();
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


















