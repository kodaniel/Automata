using Automata.Core.Contracts.Workflow;
using Automata.Runtime.Models;

namespace Automata.Runtime.Helpers;
public static class TypeExtensions
{
    public static BlockType GetBlockType(this Type type)
    {
        if (typeof(BaseEventArgs).IsAssignableFrom(type))
            return BlockType.Event;
        if (typeof(BaseActionArgs).IsAssignableFrom(type))
            return BlockType.Action;
        
        throw new NotImplementedException();
    }
}
