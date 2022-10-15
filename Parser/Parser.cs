namespace SimpleInterpreter.Parser;
using SimpleInterpreter.Runtime;


public abstract class Parser
{
    public TokenStream Tokens { get; init; }
}