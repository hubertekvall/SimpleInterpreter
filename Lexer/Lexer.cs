namespace SimpleInterpreter.Lexer;
using SimpleInterpreter.Runtime.Operators;


public struct Lexer
{
    readonly string _text;
    int _startPointer = 0;
    int _offset = 0;


    public Lexer(string text) => this._text = text;
    bool Empty() => _offset >= _text.Length;


    char Advance()
    {
        if (!Empty()) _offset++;
        return _text[_offset - 1];
    }

    char Peek()
    {
        if (!Empty()) return _text[_offset];
        else return '\0';
    }


    void StartNewToken()
    {
        _startPointer = _offset;
    }

    string GetContent()
    {
        var content = _text[_startPointer.._offset];
        return content;
    }


    public bool Chomp(Func<Char, bool> matchFunction)
    {
        if (matchFunction(Peek()))
        {
            Advance();
            return true;
        }

        return false;
    }


    public bool ChompWhile(Func<Char, bool> matchFunction)
    {
        bool status = false;
        while (matchFunction(Peek()))
        {
            Advance();
            status = true;
        }

        return status;
    }


    public Token Identifier()
    {
        ChompWhile(Char.IsLetterOrDigit);
        return IdentifierTable.GenerateIdentifier(GetContent());
    }


    public Token Comparison()
    {
        return Peek() switch
        {
            '>' => new Token(TokenType.GreaterOrEquals),
            '<' => new Token(TokenType.LesserOrEquals),
            '=' => new Token(TokenType.Equals),
            _ => new Token(TokenType.Assignment)
        };
    }


    public Token Number()
    {
        ChompWhile(Char.IsDigit);
        if (Chomp((char c) => c == '.') && !ChompWhile(Char.IsDigit))
        {
            throw new Exception("Ill-formed number literal");
        }

        return new Token(TokenType.Number, GetContent());
    }


    public Token StringLiteral(char quoteType)
    {
        if (quoteType == '\'')
        {
            ChompWhile((char c) => c != '\'');
        }

        else
        {
            ChompWhile((char c) => c != '"');
        }

        Advance();
        var content = GetContent();
        return new Token(TokenType.StringLiteral, content[1..^1]);
    }

    Token GenerateToken(TokenType type) => new Token(type, GetContent());




    Token GetNextToken()
    {

        StartNewToken();
        var currentCharacter = Advance();

        switch (currentCharacter)
        {
            case '\r':
            case '\t':
            case ' ':
                break;

            case '\n':
                return TokenType.NewLine;

            // Arithmetic operators
            case '+': return TokenType.Add;
            case '-': return TokenType.Subtract;
            case '*': return TokenType.Multiply;
            case '/': return TokenType.Divide;
            case '%': return TokenType.Mod;

            case '=': return TokenType.Assignment;

            case char ident when char.IsLetter(ident): return Identifier();
            case char number when char.IsDigit(number): return Number();
            case char quote when (quote == '"' || quote == '\''): return StringLiteral(quote);

            case '(': return TokenType.Lparen;
            case ')': return TokenType.Rparen;
        }

        throw new Exception("Invalid character");
    }


    public static List<Token> Lex(string source)
    {
        List<Token> tokens = new();
        Lexer lexer = new Lexer(source);

        while (!lexer.Empty())
        {
            var nextToken = lexer.GetNextToken();
            tokens.Add(nextToken);
        }

        return tokens;
    }



}





