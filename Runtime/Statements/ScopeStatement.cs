namespace SimpleInterpreter.Runtime;






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



public record class ConditionalStatement(IExpression Condition, IStatement Body, IStatement? ElseStatement) : ScopeStatement
{
    public override void ExecuteScope(Context context)
    {
        if (new Variant(Condition.Evaluate(context)).LogicalEval()) Body.Execute(context);
        else if (ElseStatement is not null) ElseStatement.Execute(context);
    }
}


public record class WhileStatement(IExpression Condition, IStatement Body) : ScopeStatement
{
    public override void ExecuteScope(Context context)
    {
        while (new Variant(Condition.Evaluate(context)).LogicalEval()) Body.Execute(context);
    }
}
