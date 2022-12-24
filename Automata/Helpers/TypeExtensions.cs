using Automata.Core.Contracts.Workflow;
using Automata.Plugins;

namespace Automata.Helpers;
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
