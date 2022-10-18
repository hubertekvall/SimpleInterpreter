namespace SimpleInterpreter.Runtime;


public sealed class AddOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class SubtractOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 - d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class MultiplyOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 * d2,
        (double d, string s) => string.Concat(Enumerable.Repeat(s, (int)d)),
        (string s, double d) => string.Concat(Enumerable.Repeat(s, (int)d)),
        _ => throw new NotSupportedException("Invalid operation")
    };
}

public sealed class DivideOperator : IBinaryOperator
{
    public object Execute(Object a, Object b) => (a, b) switch
    {
        (double d1, double d2) => d1 / d2,
        _ => throw new NotSupportedException("Invalid operation")
    };
}