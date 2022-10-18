namespace SimpleInterpreter.Lexer;
using SimpleInterpreter.Runtime;


public struct Lexer
{
    readonly string _text;
    int _startPointer = 0;
    int _offset = 0;


    public Lexer(string text) => this._text = text;

    bool Empty() => _offset >= _text.Length;

    char Advance() => Empty() ? _text[_offset - 1] : _text[_offset++];

    char Peek() => Empty() ? '\0' : _text[_offset];

    void StartNewToken() => _startPointer = _offset;

    string GetContent() => _text[_startPointer.._offset];



    public bool Eat(char match)
    {
        if (Peek() == match)
        {
            Advance();
            return true;
        }

        else return false;
    }


    public bool EatWhile(Func<Char, bool> matchFunction)
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
        EatWhile(Char.IsLetterOrDigit);
        return IdentifierTable.GenerateIdentifierToken(GetContent());
    }



    public Token Number()
    {
        EatWhile(Char.IsDigit);
        if (Eat('.'))
        {
            if (EatWhile(Char.IsDigit)) return new Token(TokenType.Number, GetContent());
            throw new Exception("Ill-formed number literal");
        }
        return new Token(TokenType.Number, GetContent());
    }




    public Token StringLiteral(char quoteType)
    {
        if (quoteType == '\'')
        {
            EatWhile((char c) => c != '\'');
        }

        else
        {
            EatWhile((char c) => c != '"');
        }


        Advance();

        return new Token(TokenType.StringLiteral, GetContent()[1..^1]);
    }





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


                case '(':
                    yield return TokenType.Lparen;
                    break;
                case ')':
                    yield return TokenType.Rparen;
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



                // Comparison, Relational and Logical operators
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



                // Identifiers and literals
                case char id when char.IsLetter(id):
                    yield return Identifier();
                    break;

                case char number when char.IsDigit(number):
                    yield return Number();
                    break;

                case char quote when (quote == '"' || quote == '\''):
                    yield return StringLiteral(quote);
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





