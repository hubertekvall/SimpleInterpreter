namespace SimpleInterpreter;





public sealed class Literal : IExpression
{
    public Object? Data { get; init; }

    public Variant Evaluate(Context context) => Data switch
    {
        double number => number,
        string str => str,
        _ => Empty.Value
    };
}

public sealed class Variable : IExpression
{
    public string Name { get; init; }
    public Variant Evaluate(Context context) => context.LookupVariable(Name);
}


public class AssignmentExpression : IExpression
{
    public string Identifier { get; init; }
    public IExpression Expression { get; init; }

    public Variant Evaluate(Context context)
    {
        var result = Expression.Evaluate(context);
        context.StoreVariable(Identifier, result);
        return result;
    }
}


