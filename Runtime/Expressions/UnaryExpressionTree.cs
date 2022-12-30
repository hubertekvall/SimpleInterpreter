namespace SimpleInterpreter;





public sealed class UnaryExpressionTree : IExpression
{
    public TokenType Type { get; init; }
    public IExpression Expression { get; init; }

    public  Variant Evaluate(Context context)
    {
        var operand = Expression.Evaluate(context);
        return Type switch
        {
            TokenType.Subtract => -operand,
            TokenType.Not => !operand,
            TokenType.Sqrt => operand * operand,
            _ => Empty.Value
        };
    }

}

