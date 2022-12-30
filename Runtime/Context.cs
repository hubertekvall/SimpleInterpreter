namespace SimpleInterpreter;









// Null object to be used as placeholder for any invalid actions or misconstructed trees and to avoid any null-reference exceptions
public class Empty
{
    public static readonly Empty Value = new Empty();
    public static readonly IStatement Statement = new DummyStatement();
    public static readonly IExpression Expression = new DummyExpression();


    class DummyStatement : IStatement
    {
        public void Execute(Context context) { }
    }
    class DummyExpression : IExpression
    {
        public Variant Evaluate(Context context) => Value;
    }
   ;
    public override string ToString() => "Empty object";
}










public class Scope
{
    public readonly Dictionary<string, Variant> Variables = new Dictionary<string, Variant>();
}



public class Context
{
    Stack<Scope> scopes = new Stack<Scope>();

    public Context()
    {
        PushScope();
    }


    public void PushScope()
    {
        scopes.Push(new Scope());
    }

    public void PopScope()
    {
        scopes.Pop();
    }




    public Variant LookupVariable(string name)
    {
        foreach (Scope scope in scopes.Reverse<Scope>())
        {
            if (scope.Variables.ContainsKey(name)) return scope.Variables[name];
        }

        return Empty.Value;
    }

    public void StoreVariable(string name, Variant payload)
    {
        foreach (Scope scope in scopes.Reverse<Scope>())
        {
            if (scope.Variables.ContainsKey(name)) scope.Variables[name] = payload;
        }

        scopes.Last().Variables[name] = payload;
    }
}
