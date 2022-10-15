namespace SimpleInterpreter.Runtime.Operators;

using SimpleInterpreter.Lexer;



public interface IBinaryOperator
{
    public object Execute(object A, object B);
}


public record BinaryOperator(IBinaryOperator Operator, IExpression Left, IExpression Right) : IExpression
{
    public Object Evaluate(Context context) => Operator.Execute(Left.Evaluate(context), Right.Evaluate(context));

    public static BinaryOperator MakeOperator(Token operatorToken, IExpression left, IExpression right)
    {
        IBinaryOperator op = operatorToken.Type switch
        {
            TokenType.Add => new AddOperator(),
            TokenType.Subtract => new SubtractOperator(),
            TokenType.Multiply => new MultiplyOperator(),
            TokenType.Divide => new DivideOperator(),
            _ => throw new Exception("ERROR")
        };


        return new BinaryOperator(op, left, right);
    }
}