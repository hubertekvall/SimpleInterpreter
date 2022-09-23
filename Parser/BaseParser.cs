using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimpleInterpreter;






public class BaseParser
{

    List<Token> tokenBuffer;
    int offset;

    public BaseParser(string source) => tokenBuffer = new Lexer(source).Tokenize().ToList();
    public bool Empty() => offset <= tokenBuffer.Count;

    public Token Advance()
    {
        if (!Empty()) offset++;
        if (CheckToken(TokenType.WhiteSpace) || CheckToken(TokenType.NewLine)) return Advance();
        return tokenBuffer[offset - 1];
    }

    public Token Peek() => Empty() ? tokenBuffer[offset] : tokenBuffer[offset - 1];
    public bool CheckToken(TokenType type) => Peek().type == type;


    public bool Expect(TokenType type, out Token matchedToken, string message = "")
    {
        if (Match(out matchedToken, type)) return true;
        throw new Exception(message);
    }
    
    public bool Expect(TokenType type, string message = "")
    {
        if (Match(type)) return true;
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
}


















