namespace SimpleInterpreter;









public record class BinaryOperator(Token Operator, IExpression LeftExpression, IExpression RightExpression) : IExpression
{
    public Object Evaluate(Context context)
    {
        var operandA = LeftExpression.Evaluate(context);
        var operandB = RightExpression.Evaluate(context);
        return Operator.Type switch
        {
            TokenType.Add => Add(operandA, operandB),
            TokenType.Subtract => Subtract(operandA, operandB),
            TokenType.Multiply => Multiply(operandA, operandB),
            TokenType.Divide => Divide(operandA, operandB),
            _ => throw new NotImplementedException()
        };
    }

    public static Object Add(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        _ => throw new NotSupportedException("Invalid operation")
    };


    public static Object Subtract(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 - d2,
        _ => throw new NotSupportedException("Invalid operation")
    };


    public static Object Multiply(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 * d2,
        _ => throw new NotSupportedException("Invalid operation")
    };


    public static Object Divide(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 / d2,
        _ => throw new NotSupportedException("Invalid operation")
    };

}