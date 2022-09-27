namespace SimpleInterpreter;



public interface IExpression
{
    public Object Evaluate(Context context);
}


public record class Literal(Object Payload) : IExpression
{
    public object Evaluate(Context context) => Payload;
}


public record class Variable(string Name) : IExpression
{
    public object Evaluate(Context context) => context.Load(Name);
}


public record class Assignment(string Identifier, IExpression AssignmentExpression) : IExpression
{
    public object Evaluate(Context context)
    {
        var assignmentResult = AssignmentExpression.Evaluate(context);
        context.Store(Identifier, assignmentResult);
        return assignmentResult;
    }
}



