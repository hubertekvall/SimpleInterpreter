namespace SimpleInterpreter.Lexer;
using SimpleInterpreter.Runtime;


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




    IEnumerable<Token> GetTokens()
    {
        while (!Empty())
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
                    yield return TokenType.NewLine;
                    break;

                // Arithmetic operators
                case '+':
                    yield return TokenType.Add;
                    break;
                case '-':
                    yield return TokenType.Subtract;
                    break;
                case '*':
                    yield return TokenType.Multiply;
                    break;
                case '/':
                    yield return TokenType.Divide;
                    break;
                case '%':
                    yield return TokenType.Mod;
                    break;



                // Comparison
                case '=':
                    if (Peek() == '=') yield return TokenType.Equals;
                    else yield return TokenType.Assignment;
                    break;

                case '!':
                    if (Peek() == '=') yield return TokenType.NotEquals;
                    else yield return TokenType.Not;
                    break;

                case '<':
                    if (Peek() == '=') yield return TokenType.LesserOrEquals;
                    else yield return TokenType.LesserThan;
                    break;
                case '>':
                    if (Peek() == '=') yield return TokenType.GreaterOrEquals;
                    else yield return TokenType.GreaterThan;
                    break;




                case char ident when char.IsLetter(ident):
                    yield return Identifier();
                    break;
                case char number when char.IsDigit(number):
                    yield return Number();
                    break;
                case char quote when (quote == '"' || quote == '\''):
                    yield return StringLiteral(quote);
                    break;

                case '(':
                    yield return TokenType.Lparen;
                    break;
                case ')':
                    yield return TokenType.Rparen;
                    break;

                default:
                    throw new Exception($"Invalid character: '{currentCharacter}'");
            }
        }

    }


    public static Stack<Token> Lex(string source)
    {

        Lexer lexer = new Lexer(source);
        return new Stack<Token>(lexer.GetTokens().Reverse());
    }



}





