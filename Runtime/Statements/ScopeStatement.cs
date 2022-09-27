namespace SimpleInterpreter;






public abstract record class ScopeStatement : IStatement
{
    public void Execute(Context context)
    {
        context.EnterScope();
        ExecuteScope(context);
        context.ExitScope();
    }
    public abstract void ExecuteScope(Context context);
}



public record class IfStatement(IExpression condition, IStatement body, IStatement? elseStatement) : ScopeStatement
{
    public override void ExecuteScope(Context context)
    {
        if (condition.Evaluate(context).LogicalEvaluation()) body.Execute(context);
        else if (elseStatement is not null) elseStatement.Execute(context);
    }
}
public record class IfStatement(IExpression condition, IStatement body, IStatement? elseStatement) : ScopeStatement
{
    public override void ExecuteScope(Context context)
    {
        if (condition.Evaluate(context).LogicalEvaluation()) body.Execute(context);
        else if (elseStatement is not null) elseStatement.Execute(context);
    }
}

public record class WhileStatement(IExpression condition, IStatement body) : ScopeStatement
{
    public override void ExecuteScope(Context context)
    {
        while (condition.Evaluate(context).LogicalEvaluation()) body.Execute(context);
    }
}
