using SimpleInterpreter;
using System.Diagnostics;

using SimpleInterpreter.Lexer;
using SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;

var program = ProgramParser.Parse("Hello");

program.Execute(new Context());
