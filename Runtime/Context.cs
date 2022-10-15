namespace SimpleInterpreter.Runtime;








public record struct Variant(Object Payload)
{
    public bool LogicalEval() => Payload switch
    {
        double d => (int)d > 0,
        string s => s.Length > 0,
        _ => throw new NotImplementedException()
    };
}








public class Context
{
    readonly Stack<Dictionary<string, Object>> _memory = new Stack<Dictionary<string, Object>>();


    public void EnterScope() => _memory.Push(new Dictionary<string, object>());
    public void ExitScope() => _memory.Pop();




    public Object Load(string identifier)
    {
        foreach (var dict in _memory.Reverse())
        {
            if (dict.TryGetValue(identifier, out Object? value))
            {
                return value;
            }
        }

        return "N/A";
    }

    public void Store(string identifier, Object value)
    {
        foreach (var dict in _memory.Reverse())
        {
            if (dict.ContainsKey(identifier))
            {
                dict[identifier] = value;
                return;
            }
        }
        _memory.Last().Add(identifier, value);
    }


}

