using SimpleInterpreter;
using System.Diagnostics;

using static SimpleInterpreter.TokenType;


var program = new ProgramParser(File.ReadAllText("..\\..\\..\\sample.txt")).Program();
program.Execute(new Context());