namespace SimpleInterpreter;
using System;
using System.Runtime.InteropServices;






public struct Variant
{
    public enum Type
    {
        Number,
        String,
        Dict,
        None
    }


    // [StructLayout(LayoutKind.Explicit)]
    public struct Data
    {
        // [FieldOffset(0)] 
        public double Number;
        // [FieldOffset(0)] 
        public string String;
        // [FieldOffset(0)] 
        public Dictionary<string, Variant> Dict;
        // [FieldOffset(0)] 
        public Empty None;
        public static implicit operator Data(double number) => new Data { Number = number };
        public static implicit operator Data(string str) => new Data { String = str };
        public static implicit operator Data(Empty none) => new Data { None = none };
    }


    Data payload;
    Type tag;


    public string StringValue
    {
        get { return payload.String; }
        set
        {
            payload.String = value;
            tag = Type.String;
        }
    }

    public double NumberValue
    {
        get { return payload.Number; }
        set
        {
            payload.Number = value;
            tag = Type.Number;
        }
    }

    public Dictionary<string, Variant> DictValue
    {
        get { return payload.Dict; }
        set
        {
            payload.Dict = value;
            tag = Type.Dict;
        }
    }
    public Empty NoneValue
    {
        get { return payload.None; }
        set
        {
            payload.None = Empty.Value;
            tag = Type.None;
        }
    }



    public Variant(double number) => NumberValue = number;
    public Variant(string str) => StringValue = str;
    public Variant(Dictionary<string, Variant> dict) => DictValue = dict;
    public Variant(Empty none) => NoneValue = none;






    public static implicit operator Variant(double number) => new Variant(number);
    public static implicit operator Variant(string str) => new Variant(str);
    public static implicit operator Variant(Empty none) => new Variant(none);
    public static implicit operator Variant(bool boolean) => new Variant(boolean ? 1 : 0);





    public override string ToString()
    {
        return tag switch
        {
            Type.Number => NumberValue.ToString(),
            Type.Dict => DictValue.ToString(),
            Type.String => StringValue,
            _ => NoneValue.ToString()
        };
    }


    public bool IsTrue() => tag switch
    {
        Type.Number => NumberValue > 0,
        Type.String => StringValue.Length > 0,
        _ => false
    };




    // Arithmetic operators
    public static Variant operator +(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue + b.NumberValue,
        (Variant.Type.Number, Variant.Type.String) => string.Join("", a.NumberValue, b.StringValue),
        (Variant.Type.String, Variant.Type.Number) => string.Join("", a.StringValue, b.NumberValue),
        (Variant.Type.String, Variant.Type.String) => string.Join("", a.StringValue, b.StringValue),
        _ => Empty.Value
    };

    public static Variant operator -(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue - b.NumberValue,
        _ => Empty.Value
    };



    public static Variant operator /(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue / a.NumberValue,
        _ => Empty.Value
    };

    public static Variant operator *(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue * a.NumberValue,
        _ => Empty.Value
    };





    // Comparison operators
    public static Variant operator <(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue * a.NumberValue,
        _ => Empty.Value
    };
    public static Variant operator >(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue > a.NumberValue,
        _ => Empty.Value
    };
    public static Variant operator >=(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue >= a.NumberValue,
        _ => Empty.Value
    };
    public static Variant operator <=(Variant a, Variant b) => (a.tag, b.tag) switch
    {
        (Variant.Type.Number, Variant.Type.Number) => a.NumberValue <= a.NumberValue,
        _ => Empty.Value
    };





    // Unary operators
    public static Variant operator -(Variant a) => (a.tag) switch
    {
        (Variant.Type.Number) => -a.NumberValue,
        _ => Empty.Value
    };


    public static Variant operator !(Variant a) => (a.tag) switch
    {
        _ => !a
    };

}
