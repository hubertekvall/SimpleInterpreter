namespace SimpleInterpreter.Runtime;
using SimpleInterpreter.Lexer;



public interface IUnaryOperator
{
    public object Execute(object expression);
}



public class UnaryOperatorExpression : IExpression
{
    IExpression Expression { get; init; }
    IUnaryOperator Operator { get; set; }

    public UnaryOperatorExpression(Token operatorToken, IExpression expression)
    {
        Expression = expression;
        Operator = OperatorTokens.UnaryOperators[operatorToken];
    }



    public object Evaluate(Context context)
    {
        return Operator.Execute(Expression.Evaluate(context));
    }
}