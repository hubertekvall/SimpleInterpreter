namespace SimpleInterpreter;









// public struct Variant
// {
//     [StructLayout(LayoutKind.Explicit)]
//     public struct Data
//     {
//         [FieldOffset(0)] private double f64;
//         [FieldOffset(0)] private object obj;



//         public double F64 { get { return f64} set { f64 = value} }
//         public object Obj { get { return obj} set { obj = value} }
//     }


//     public enum Flag
//     {
//         Number,
//         String,
//         Boolean,
//         Integer,
//         Object
//     }


//     Flag Tag { get; init; }
//     Data Payload { get; init; }


//     public Variant(double f64)
//     {
//         Payload = new Data();
//         Payload.F64 = f64;
//         Tag = Flag.Number;
//     }
//     public Variant(object obj)
//     {
//         Payload = new Data();
//         Payload.Obj = obj;
//         Tag = Flag.Number;
//     }


//     public static implicit operator Variant.Flag(Variant t) => Tag;
//     public static implicit operator Variant(object obj) => obj switch
//     {
//         double d => new Variant(d),
//         object o => new Variant(o)
//     };
// }





public record struct Variant(Object Payload)
{
    public bool LogicalEval() => Payload switch
    {
        double d => (int)d > 0,
        string s => s != null && s.Length > 0,
        _ => throw new NotImplementedException()
    };
}








public class Context
{

    Stack<Dictionary<string, Object>> memory = new Stack<Dictionary<string, Object>>();


    public void EnterScope() => memory.Push(new Dictionary<string, object>());
    public void ExitScope() => memory.Pop();




    public Object Load(string identifier)
    {
        foreach (var dict in memory.Reverse())
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
        foreach (var dict in memory.Reverse())
        {
            if (dict.ContainsKey(identifier))
            {
                dict[identifier] = value;
                return;
            }
        }
        memory.Last().Add(identifier, value);
    }


}

