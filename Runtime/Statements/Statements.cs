namespace SimpleInterpreter;



public interface IStatement
{
    public void Execute(Context context);
}


public record class ExpressionStatement(IExpression Expression) : IStatement
{
    public void Execute(Context context) => Expression.Evaluate(context);
}


public record class BlockStatement(List<IStatement> Statements) : IStatement
{
    public void Execute(Context context)
    {
        foreach (var statement in Statements)
        {
            statement.Execute(context);
        }
    }
}