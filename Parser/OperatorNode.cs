namespace SimpleInterpreter;






public record class OperatorNode(Token Operator, OperatorNode? Left, OperatorNode? Right, Object? Data)
{

    public void Execute(Context context)
    {
        
    }


    public Object Evaluate(Context context)
    {

    }
}