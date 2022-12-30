namespace SimpleInterpreter;







public sealed class BinaryExpressionTree : IExpression
{
    public TokenType Type { get; init; }
    public IExpression Left { get; init; }
    public IExpression Right { get; init; }





    public  Variant Evaluate(Context context)
    {
        var LeftOperand = () => Left.Evaluate(context);
        var RightOperand = () => Right.Evaluate(context);

        return Type switch
        {
            TokenType.Add => LeftOperand() + RightOperand(),
            TokenType.Subtract => LeftOperand() - RightOperand(),
            TokenType.Divide => LeftOperand() / RightOperand(),
            TokenType.Multiply => LeftOperand() * RightOperand(),

            TokenType.LesserOrEquals => LeftOperand() <= RightOperand(),
            TokenType.LesserThan => LeftOperand() < RightOperand(),
            TokenType.GreaterOrEquals => LeftOperand() >= RightOperand(),
            TokenType.GreaterThan => LeftOperand() > RightOperand(),

            _ => Empty.Value
        };
    }

}


