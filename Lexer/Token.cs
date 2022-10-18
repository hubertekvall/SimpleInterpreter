namespace SimpleInterpreter.Lexer;

using SimpleInterpreter.Runtime;




public struct OperatorTokens
{
    public static readonly Dictionary<TokenType, IBinaryOperator> BinaryOperators = new()
    {
        {TokenType.Add, new AddOperator ()},
        {TokenType.Subtract, new SubtractOperator()},
        {TokenType.Multiply , new MultiplyOperator()},
        {TokenType.Divide , new DivideOperator()},
        {TokenType.LesserThan , new LesserThanOperator()},
        {TokenType.LesserOrEquals , new LesserOrEqualsOperator()},
        {TokenType.GreaterThan , new GreaterThanOperator()},
        {TokenType.GreaterOrEquals , new GreaterOrEqualsOperator()},
        {TokenType.NotEquals , new NotEqualsOperator()},
        {TokenType.Equals , new EqualsOperator()},
    };
    public static readonly Dictionary<TokenType, IUnaryOperator> UnaryOperators = new()
    {
        {TokenType.Sqrt, new SquareRootOperator()},
        {TokenType.Subtract, new NegationOperator()},
        {TokenType.Print, new PrintOperator()},
    };
}



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
    public static Token GenerateIdentifier(string content) => Names.TryGetValue(content, out TokenType identifiedType) ? new(identifiedType, content) : new(TokenType.Identifier, content);
}

public record struct Token(TokenType Type, string Content = "")
{
    public static implicit operator TokenType(Token token) => token.Type;
    public static implicit operator Token(TokenType type) => new Token(type);
    public override string ToString()
    {
        return Type.ToString();
    }

    public bool Equals(Token B) => this.Type == B.Type;
}

