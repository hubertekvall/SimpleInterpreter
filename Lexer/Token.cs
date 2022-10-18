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
}



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
    And,
    Or,
    NewLine,
    WhiteSpace
}


public class IdentifierTable
{
    static readonly Dictionary<string, TokenType> Names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"WHILE", TokenType.While},
            {"THEN", TokenType.Then},
            {"END", TokenType.End },
            {"and", TokenType.And },
            {"or", TokenType.Or },
            {"PRINT", TokenType.PrintStatement}
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

