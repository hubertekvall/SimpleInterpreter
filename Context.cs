using System;
using System.Collections.Generic;


namespace SimpleInterpreter;



// public struct MemoryHandle
// {
//     public Object value;
//     public int scopeLevel;

//     public MemoryHandle(int scopeLevel, Object value)
//     {
//         this.value = value;
//         this.scopeLevel = scopeLevel;
//     }
// }



public class Context
{

    Stack<Dictionary<string, Object>> memory = new Stack<Dictionary<string, Object>>();



    public void EnterScope()
    {
        memory.Push(new Dictionary<string, object>());
    }

    public void ExitScope()
    {
        memory.Pop();
    }

    public Object Load(string identifier)
    {
        Object value;

        foreach (var dict in memory)
        {
            if (dict.TryGetValue(identifier, out value))
            {
                return value;
            }
        }

        return "N/A";
    }

    public void Store(string identifier, Object value)
    {
        foreach (var dict in memory)
        {
            if (dict.ContainsKey(identifier))
            {
                dict[identifier] = value;
                return;
            }
        }


        else memory.Last().Add(identifier, value);
    }


}

