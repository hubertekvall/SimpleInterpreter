namespace SimpleInterpreter.Lexer;

using SimpleInterpreter.Runtime.Operators;

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
     static readonly Dictionary<string, TokenType> Names = new Dictionary<string, TokenType>
        {
            {"IF", TokenType.If },
            {"ELSEIF", TokenType.ElseIf},
            {"ELSE", TokenType.Else },
            {"WHILE", TokenType.While},
            {"THEN", TokenType.Then},
            {"END", TokenType.End },
            {"PRINT", TokenType.PrintStatement}
        };
    public static Token GenerateIdentifier(string content) => Names.TryGetValue(content, out TokenType identifiedType) ? new(identifiedType, content) : new(TokenType.Identifier, content);
}

public record class Token(TokenType Type, string Content = "")
{
    public static implicit operator TokenType(Token token) => token.Type;
    public static implicit operator Token(TokenType type) => new Token(type);
    public override string ToString()
    {
        return Type.ToString();
    }
}

public record class OperatorToken(IBinaryOperator Operator, TokenType Type, string Content = "") : Token(Type, Content);

