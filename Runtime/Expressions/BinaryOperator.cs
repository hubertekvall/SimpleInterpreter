namespace SimpleInterpreter;







public record class BinaryOperator(Token Operator, IExpression LeftExpression, IExpression RightExpression) : IExpression
{


    public Object Evaluate(Context context)
    {
        var A = LeftExpression.Evaluate(context);
        var B = RightExpression.Evaluate(context);

        return Operator.Type switch
        {
            TokenType.Add => Add(A, B),
            TokenType.Subtract => Subtract(A, B),
            TokenType.Multiply => Multiply(A, B),
            TokenType.Divide => Divide(A, B),
            TokenType.GreaterOrEquals => GreaterOrEquals(A, B),
            TokenType.LesserOrEquals => LesserOrEquals(A, B),
            TokenType.Equals => Equals(A, B),
            _ => throw new NotImplementedException("Parsing error, not a valid binary operator")
        };
    }









    // COMPARISON

    public static Object GreaterOrEquals(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };


    public static Object LesserOrEquals(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };

    public static Object GreaterThan(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };

    public static Object LesserThan(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };



    // ARITHMETIC
    public static Object Add(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
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