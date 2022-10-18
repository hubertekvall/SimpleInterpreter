namespace SimpleInterpreter.Runtime;






public sealed class NegationOperator : IUnaryOperator
{
    public object Execute(Object A) => A switch
    {
        double d => -d,
        _ => throw new NotSupportedException("Invalid type for operation")
    };
}
public sealed class SquareRootOperator : IUnaryOperator
{
    public object Execute(Object A) => A switch
    {
        double d => Math.Sqrt(d),
        _ => throw new NotSupportedException("Invalid type for operation")
    };
}