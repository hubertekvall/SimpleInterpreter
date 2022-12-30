using System;
namespace SimpleInterpreter;




public interface IStatement
{
    public void Execute(Context context);
}



public interface IExpression
{
    public  Variant Evaluate(Context context);
}
