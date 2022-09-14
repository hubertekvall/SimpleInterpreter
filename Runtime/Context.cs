using System;
using System.Collections.Generic;


namespace SimpleInterpreter;




public class Context
{

    Stack<Dictionary<string, Object>> memory = new Stack<Dictionary<string, Object>>();



    public void EnterScope() => memory.Push(new Dictionary<string, object>());
    

    public void ExitScope() => memory.Pop();


    public Object Load(string identifier)
    {
  

        foreach (var dict in memory.Reverse())
        {
            if (dict.TryGetValue(identifier, out Object value))
            {
                return value;
            }
        }

        return "N/A";
    }

    public void Store(string identifier, Object value)
    {
        foreach (var dict in memory.Reverse())
        {
            if (dict.ContainsKey(identifier))
            {
                dict[identifier] = value;
                return;
            }
        }
        memory.Last().Add(identifier, value);
    }


}

