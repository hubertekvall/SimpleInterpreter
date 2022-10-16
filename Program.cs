using SimpleInterpreter;
using System.Diagnostics;

using SimpleInterpreter.Lexer;
using SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;

var sampleCode = File.ReadAllText("sample.txt");
var program = ProgramParser.Parse(sampleCode);
program.Execute(new Context());
