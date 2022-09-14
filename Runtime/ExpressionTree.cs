namespace SimpleInterpreter;








public interface IExpression
{
    public Object Evaluate(Context context);
}

public interface IStatement
{
    public void Execute(Context context);
}





public class BinaryOperator : IExpression
{

    IExpression leftExpression;
    IExpression rightExpression;
    Token op;

    public Object Evaluate(Context context)
    {
        var opA = leftExpression.Evaluate(context);
        var opB = rightExpression.Evaluate(context);
        var result = op.type switch
        {
            TokenType.Add => Add(opA, opB),
            TokenType.Subtract => Subtract(opA, opB),
            TokenType.Multiply => Multiply(opA, opB),
            TokenType.Divide => Divide(opA, opB),
            _ => throw new NotSupportedException("Invalid token or operator")
        };
        return result;
    }

    public BinaryOperator(Token token, IExpression left, IExpression right)
    {
        this.leftExpression = left;
        this.rightExpression = right;
        this.op = token;
    }


    public Object Add(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 + d2,
            (double d1, string s2) => string.Join("", d1, s2),
            (string s1, double d2) => string.Join("", s1, d2),
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public Object Subtract(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 - d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public Object Multiply(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 * d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public Object Divide(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 / d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }
}




public class UnaryOperator : IExpression
{
    IExpression expression;
    Token op;

    public UnaryOperator(Token token, IExpression expression)
    {
        this.op = token;
        this.expression = expression;
    }

    public Object Evaluate(Context context)
    {
        var operandA = expression.Evaluate(context);
        return op.type switch
        {
            TokenType.Not => Not(operandA),
            TokenType.Subtract => Negate(operandA),
            _ => throw new NotSupportedException("Invalid operator")
        };
    }

    public static Object Not(Object v1)
    {
        return v1 switch
        {
            bool b => !b,
            _ => throw new NotSupportedException("Invalid type for operation")
        };
    }

    public static Object Negate(Object v1)
    {
        return v1 switch
        {
            double d => -d,
            _ => throw new NotSupportedException("Invalid type for operation")
        };
    }
}



public class Literal : IExpression
{
    Object payload;

    public Literal(Object payload) => this.payload = payload;
    public object Evaluate(Context context) => payload;
}


public class Load : IExpression
{
    string identifier;
    public Load(string identifier) => this.identifier = identifier;
    public object Evaluate(Context context) => context.Load(identifier);
}




public class AssignmentStatement : IStatement
{
    string identifier;
    IExpression expression;

    public AssignmentStatement(string identifier, IExpression expression)
    {
        this.identifier = identifier;
        this.expression = expression;
    }

    public void Execute(Context context) => context.Store(identifier, expression.Evaluate(context));
}



public class ExpressionStatement : IStatement
{
    IExpression expression;
    public ExpressionStatement(IExpression expression) => this.expression = expression;
    public void Execute(Context context) => expression.Evaluate(context);
}


public class IfStatement : IStatement
{
    IExpression condition;
    IStatement body;
    IStatement elseStatement;


    public IfStatement(IExpression condition, IStatement body, IStatement elseStatement)
    {
        this.condition = condition;
        this.body = body;
        this.elseStatement = elseStatement;
    }


    public void Execute(Context context)
    {
        if (condition.Evaluate(context).LogicalEvaluation()) body.Execute(context);

        else if (elseStatement is not null) elseStatement.Execute(context);
    }
}




public class BlockStatement : IStatement
{
    List<IStatement> statements;

    public BlockStatement(List<IStatement> statements) => this.statements = statements;
    public void Execute(Context context)
    {
        foreach (var statement in statements)
        {
            statement.Execute(context);
        }
    }
}