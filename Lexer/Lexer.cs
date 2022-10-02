namespace SimpleInterpreter;



public enum TokenType
{
    PrintStatement,
    EndOfText,
    Identifier,
    Lparen,
    Rparen,
    Add,
    Subtract,
    Multiply,
    Divide,
    Mod,
    Number,
    StringLiteral,
    VariableDeclaration,
    Assignment,
    Equals,
    NotEquals,
    GreaterOrEquals,
    LesserOrEquals,
    LesserThan,
    GreaterThan,
    Not,
    If,
    ElseIf,
    Else,
    End,
    Then,
    While,
    For,
    NewLine,
    WhiteSpace
}


public class IdentifierTable
{
    public static Dictionary<string, TokenType> names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"WHILE", TokenType.While},
            {"THEN", TokenType.Then},
            {"END", TokenType.End },
            {"PRINT", TokenType.PrintStatement}
        };
    public static Token GenerateIdentifier(string content) => names.TryGetValue(content, out TokenType identifiedType) ? new(identifiedType, content) : new(TokenType.Identifier, content);
}

public record struct Token(TokenType Type, string Content = "")
{
    public static implicit operator TokenType(Token token) => token.Type;
    public static implicit operator Token(TokenType type) => new Token(type);
    public override string ToString()
    {
        return Type.ToString();
    }
}











public struct Lexer
{
    readonly string text;
    int startPointer = 0;
    int offset = 0;



    public Lexer(string text) => this.text = text;
    bool Empty() => offset >= text.Length;


    char Advance()
    {
        if (!Empty()) offset++;
        return text[offset - 1];
    }

    char Peek()
    {
        if (!Empty()) return text[offset];
        else return '\0';
    }



    void StartNewToken()
    {
        startPointer = offset;
    }
    string GetContent()
    {
        var content = text[startPointer..offset];
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


    public List<Token> GetTokens()
    {
        List<Token> tokens = new();

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
                    tokens.Add(GenerateToken(TokenType.NewLine));
                    break;

                // Arithmetic operators
                case '+':
                    tokens.Add(GenerateToken(TokenType.Add));
                    break;
                case '-':
                    tokens.Add(GenerateToken(TokenType.Subtract));
                    break;

                case '*':
                    tokens.Add(GenerateToken(TokenType.Multiply));
                    break;

                case '/':
                    tokens.Add(GenerateToken(TokenType.Divide));
                    break;

                case '%':
                    tokens.Add(GenerateToken(TokenType.Mod));
                    break;

                case '=':
                    tokens.Add(GenerateToken(TokenType.Assignment));
                    break;





                case char ident when char.IsLetter(ident):
                    tokens.Add(Identifier());
                    break;

                case char number when char.IsDigit(number):
                    tokens.Add(Number());
                    break;

                case char quote when (quote == '"' || quote == '\''):
                    tokens.Add(StringLiteral(quote));
                    break;





                case '(':
                    tokens.Add(GenerateToken(TokenType.Lparen));
                    break;

                case ')':
                    tokens.Add(GenerateToken(TokenType.Rparen));
                    break;


                default:
                    throw new Exception("Invalid character");
            }

        }

        return tokens;
    }





}



