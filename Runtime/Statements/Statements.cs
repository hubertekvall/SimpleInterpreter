namespace SimpleInterpreter;



public abstract class ScopedTree : IStatement
{
    public void Execute(Context context)
    {
        context.PushScope();
        ExecuteScope(context);
        context.PopScope();
    }
    public abstract void ExecuteScope(Context context);
}




public sealed class BlockTree : IStatement
{
    public List<IStatement> Statements { get; init; }


    public void Execute(Context context)
    {
        foreach (var statement in Statements)
        {
            statement.Execute(context);
        }
    }
}



public sealed class WhileStatementTree : ScopedTree
{
    public IExpression Conditional { get; init; }
    public IStatement Body { get; init; }

    public override void ExecuteScope(Context context)
    {
        while (Conditional.Evaluate(context).IsTrue())
        {
            Body.Execute(context);
        }
    }
}


public sealed class ConditionalStatementTree : ScopedTree
{
    public IExpression Conditional { get; init; }
    public IStatement MainBranch { get; init; }
    public IStatement ElseBranch = Empty.Statement;

    public override void ExecuteScope(Context context)
    {
        // If statement was succesful, execute the main branch
        if (Conditional.Evaluate(context).IsTrue()) MainBranch.Execute(context);

        // Try to execute the else-branch
        else ElseBranch.Execute(context);
    }
}


public sealed class ExpressionStatement : IStatement
{
    public IExpression Expression { get; init; }
    public void Execute(Context context)
    {
        var result = Expression.Evaluate(context);


#if DEBUG
        Console.WriteLine(result);
#endif

    }
}