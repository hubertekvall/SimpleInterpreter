namespace SimpleInterpreter.Runtime;

using SimpleInterpreter.Lexer;


public interface IBinaryOperator
{
    public object Execute(Object A, Object B);
}


public class BinaryOperatorExpression : ExpressionTree
{
    IBinaryOperator Operator { get; set; }

    public BinaryOperatorExpression(Token operatorToken, IExpression left, IExpression right)
    {
        Operator = OperatorTokens.BinaryOperators[operatorToken];
        Left = left;
        Right = right;
    }

    public override object Evaluate(Context context) => Operator.Execute(Left!.Evaluate(context), Right!.Evaluate(context));
}