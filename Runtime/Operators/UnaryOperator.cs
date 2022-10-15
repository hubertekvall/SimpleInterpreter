namespace SimpleInterpreter.Runtime.Operators;
using SimpleInterpreter.Lexer;




public record class UnaryOperator(Token Operator, IExpression Expression) : IExpression
{
    public Object Evaluate(Context context)
    {
        var operandA = Expression.Evaluate(context);
        return Operator.Type switch
        {
            TokenType.Not => Not(operandA),
            TokenType.Subtract => Negate(operandA),
            _ => throw new NotSupportedException("Invalid operator")
        };
    }

    public Object Not(Object v1)
    {
        return v1 switch
        {
            bool b => !b,
            _ => throw new NotSupportedException("Invalid type for operation")
        };
    }

    public Object Negate(Object v1)
    {
        return v1 switch
        {
            double d => -d,
            _ => throw new NotSupportedException("Invalid type for operation")
        };
    }
}