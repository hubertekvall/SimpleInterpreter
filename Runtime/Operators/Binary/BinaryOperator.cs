namespace SimpleInterpreter.Runtime.Operators;

using SimpleInterpreter.Lexer;



public interface IBinaryOperator
{
    public object Execute(IExpression A, IExpression B);
}


public class BinaryOperator : IExpression
{

    IExpression Left { get; set; }
    IExpression Right { get; set; }

    public Object Evaluate(Context context) => Operator.Execute(Left, Right);

    public BinaryOperator(Token operatorToken, IExpression left, IExpression right)
    {
        Operator = TokenOperators.BinaryOperators[operatorToken.Type];
        Left = left;
        Right = right;
    }
}