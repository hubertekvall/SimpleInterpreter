using System;
using System.IO;
using SimpleInterpreter;



var context = new Context();
context.EnterScope();

try
{

    var tree = Parser.ParseCode(File.ReadAllText("sample.txt"));
}
catch (System.Exception e)
{

    Console.WriteLine(e.Message);
}




