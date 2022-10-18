namespace SimpleInterpreter.Runtime;



public sealed class AndExpression : ExpressionTree
{
    public override object Evaluate(Context context)
    {
        return Left!.Evaluate(context).IsTrue() && Right!.Evaluate(context).IsTrue();
    }
}

public sealed class OrExpression : ExpressionTree
{
    public override object Evaluate(Context context)
    {
        return Left!.Evaluate(context).IsTrue() || Right!.Evaluate(context).IsTrue();
    }
}