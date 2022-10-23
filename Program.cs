using SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;


public static class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0) Console.WriteLine("Need source file");

        else
        {
            var sampleCode = File.ReadAllText(args[0]);
            var program = ProgramParser.Parse(sampleCode);
            program.Execute(new Context());
        }

    }
}

