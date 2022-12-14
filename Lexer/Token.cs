namespace SimpleInterpreter;

using SimpleInterpreter;





public enum TokenType
{
    Print,
    Sqrt,

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
    And,
    Or,
    NewLine
}


public class IdentifierTable
{
    public static readonly Dictionary<string, TokenType> Names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"WHILE", TokenType.While},
            {"THEN", TokenType.Then},
            {"END", TokenType.End },
            {"AND", TokenType.And },
            {"OR", TokenType.Or },
            {"PRINT", TokenType.Print},
            {"SQRT", TokenType.Sqrt},
        };
    public static TokenType GetIdentifier(string content) => Names.TryGetValue(content, out TokenType identifiedType) ? identifiedType : TokenType.Identifier;
}

public record struct Token(TokenType Type, int Line = 0, string Content = "" )
{
    public static implicit operator TokenType(Token token) => token.Type;
    public static implicit operator Token(TokenType type) => new Token(type);
    public override string ToString()
    {
        return Type.ToString();
    }

    public bool Equals(Token B) => this.Type == B.Type;
}

