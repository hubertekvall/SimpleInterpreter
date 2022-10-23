namespace SimpleInterpreter.Runtime;

public sealed class PrintOperator : IUnaryOperator
{
    public object Execute(Object A)
    {
        Console.WriteLine(A);
        return A;
    }
}