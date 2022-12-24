using Automata.Core.Helpers;
using Automata.Daemon.Helpers;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Automata.Daemon.Models;

[DebuggerDisplay("{Name}")]
public class Block
{
    public Block(Type type)
    {
        Guard.AgainstNull(type);

        UniqueId = type.AssemblyQualifiedName!;
        Type = type;
        Name = type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name.ToTitleCase();
        Description = type.GetCustomAttribute<DescriptionAttribute>()?.Description;
        BlockType = Type.GetBlockType();
    }

    public string UniqueId { get; }

    public Type Type { get; }

    public BlockType BlockType { get; }

    public string Name { get; }

    public string? Description { get; }
}
