namespace SimpleInterpreter.Runtime.Operators;



public sealed class EqualsOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 == d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class NotEqualsOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 != d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class AndOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (bool d1, bool d2) => d1 && d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class OrOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (bool d1, bool d2) => d1 || d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}