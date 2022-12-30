using SimpleInterpreter;


public static class Program
{
    static void Main(string[] args)
    {
        // if (args.Length == 0) Console.WriteLine("Need source file");

        // else
        // {
            var sampleCode = File.ReadAllText("sample.txt");
            var program = ProgramParser.Parse(sampleCode);
            program.Execute(new Context());
        // }

    }
}

