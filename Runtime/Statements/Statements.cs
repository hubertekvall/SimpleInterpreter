namespace SimpleInterpreter;



public interface IStatement
{
    public void Execute(Context context);
}


public record class ExpressionStatement(IExpression Expression) : IStatement
{
    public void Execute(Context context)
    {
        var result = Expression.Evaluate(context);
        Console.WriteLine(result);
    }
}


public record class BlockStatement(List<IStatement> Statements) : ScopeStatement
{

    public override void ExecuteScope(Context context)
    {
        foreach (var statement in Statements)
        {
            statement.Execute(context);
        }
    }
}


public class EmptyStatement : IStatement
{
    public void Execute(Context context)
    {
       
    }
}
