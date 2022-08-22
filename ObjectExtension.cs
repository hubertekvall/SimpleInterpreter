public static class ObjectExtension
{



    public static bool LogicalEvaluation(this object value) => value switch
    {
        double d => ((int)d) > 0,
        bool b => b,
        _ => false
    };

}