using System;
using System.IO;
using SimpleInterpreter;



var tree = Parser.ParseCode(File.ReadAllText("sample.txt"));
var context = new Context();
context.EnterScope();


Console.WriteLine(tree.Execute(context));




