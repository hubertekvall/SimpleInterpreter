namespace SimpleInterpreter.Runtime;


public interface IExpression
{
    public Object Evaluate(Context context);
}

public abstract class ExpressionTree : IExpression
{
    public IExpression? Left { get; init; }
    public IExpression? Right { get; init; }
    public abstract object Evaluate(Context context);
}



public record class Literal(Object Payload) : IExpression
{
    public object Evaluate(Context context) => Payload;
}


public record class Variable(string Name) : IExpression
{
    public object Evaluate(Context context) => context.Load(Name);
}


public record class AssignmentExpression(string Identifier, IExpression Expression) : IExpression
{
    public object Evaluate(Context context)
    {
        var result = Expression.Evaluate(context);
        context.Store(Identifier, result);
        return result;
    }
}


