namespace SimpleInterpreter.Runtime;



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







