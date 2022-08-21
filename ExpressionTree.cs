namespace SimpleInterpreter;














public abstract class TreeNode
{
    protected TreeNode left;
    protected TreeNode right;


    public TreeNode()
    {
        this.left = new Empty();
        this.right = new Empty();
    }

    public TreeNode(TreeNode left)
    {
        this.left = left;
        this.right = new Empty();
    }

    public TreeNode(TreeNode left, TreeNode right)
    {
        this.left = left;
        this.right = right;
    }

    public abstract Object Execute(Context context);
  
}


public class Empty : TreeNode
{
    public override Object Execute(Context context)
    {
        return "N/A";
    }
}



public class BinaryOperator : TreeNode
{

    Func<Object, Object, Object> operatorFunction;

    public override Object Execute(Context context)
    {
        var opA = left.Execute(context);
        var opB = right.Execute(context);
        var result = operatorFunction(opA, opB);

        return result;
    }

    public BinaryOperator(Token token, TreeNode left, TreeNode right) : base(left, right)
    {

        this.operatorFunction = token.type switch
        {
            TokenType.Add => Add,
            TokenType.Subtract => Subtract,
            TokenType.Multiply => Multiply,
            TokenType.Divide => Divide,
            _ => throw new NotSupportedException("Invalid operator")
        };

    }


    public static Object Add(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 + d2,
            (double d1, string s2) => string.Join("", d1, s2),
            (string s1, double d2) => string.Join("", s1, d2),
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public static Object Subtract(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 - d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public static Object Multiply(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 * d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }

    public static Object Divide(Object v1, Object v2)
    {
        return (v1, v2) switch
        {
            (double d1, double d2) => d1 / d2,
            _ => throw new NotSupportedException("Invalid operation")
        };
    }
}




public class UnaryOperator : TreeNode
{
    Func<Object, Object> operatorFunction;


    public UnaryOperator(Token token, TreeNode left) : base(left)
    {
        this.operatorFunction = token.type switch
        {
            TokenType.Not => Not,
            TokenType.Subtract => Negate,
            _ => throw new NotSupportedException("Invalid operator")
        };

    }

    public override Object Execute(Context context)
    {
        var operandA = left.Execute(context);

        return operatorFunction(operandA);
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



public class Literal : TreeNode
{

    Object payload;

    public Literal(Object payload)
    {
        this.payload = payload;
    }

    public override object Execute(Context context)
    {
        return payload;
    }

}


public class LoadVariable : TreeNode
{

    string identifier;

    public LoadVariable(string identifier)
    {
        this.identifier = identifier;
    }

    public override object Execute(Context context)
    {
        return context.Load(identifier);
    }
}




public class AssignmentStatement : TreeNode
{

    public AssignmentStatement(TreeNode identifier, TreeNode expression)
    {
        this.left = identifier;
        this.right = expression;
    }

    public override object Execute(Context context)
    {
        var identifier = left.Execute(context) as string;
        var expression = right.Execute(context);
        context.Store(identifier, expression);


        return expression;
    }
}





