namespace SimpleInterpreter.Runtime.Operators;



public sealed class EqualsOperator : IExpression
{
    public object Evaluate(Context context)
    {
        throw new NotImplementedException();
    }

    public object Execute(IExpression a, IExpression b) => (, b) switch
    {
        (double d1, double d2) => Math.Round(d1) == Math.Round(d2),
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class NotEqualsOperator : IBinaryOperator
{
    public object Execute(IExpression a, IExpression b) => (a.Eva, b) switch
    {
        (double d1, double d2) => Math.Round(d1) != Math.Round(d2),
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class AndOperator : IBinaryOperator
{
    public object Execute(IExpression a, IExpression b) => (a, b) switch
    {
        (bool d1, bool d2) => d1 && d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class OrOperator : IBinaryOperator
{
    public object Execute(IExpression a, IExpression b) => (a, b) switch
    {
        (bool d1, bool d2) => d1 || d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}