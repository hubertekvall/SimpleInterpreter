namespace SimpleInterpreter.Runtime;
using SimpleInterpreter.Lexer;


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





public record class AssignmentExpression(string Identifier, IExpression Expression) : IExpression
{
    public object Evaluate(Context context)
    {
        var result = Expression.Evaluate(context);
        context.Store(Identifier, result);
        return result;
    }
}



