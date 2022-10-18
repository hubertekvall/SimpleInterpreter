namespace SimpleInterpreter.Runtime;


public record struct Variant(Object? Payload)
{


    public bool IsTrue() => Payload switch
    {
        double d => (int)d > 0,
        bool b => b,
        string s => s.Length > 0,
        _ => throw new NotImplementedException()
    };


    public static Variant operator +(Variant a, Variant b) => (a.Payload, b.Payload) switch
    {
        (double d1, double d2) => d1 + d2,
        (double d1, string s2) => d1 + s2,
        (string s1, double d2) => s1 + d2,
        (string s1, string s2) => s1 + s2,
        _ => throw new NotSupportedException("Invalid operation")
    };

    public static Variant operator -(Variant a, Variant b) => (a.Payload, b.Payload) switch
    {
        (double d1, double d2) => d1 - d2,
        _ => throw new NotSupportedException("Invalid operation")
    };


    public static implicit operator Variant(double number) => new Variant(number);
    public static implicit operator Variant(string str) => new Variant(str);
    public static implicit operator Variant(bool boolean) => new Variant(boolean);


    public static Variant UndefinedVariant()
    {
        return new Variant(null);
    }

}