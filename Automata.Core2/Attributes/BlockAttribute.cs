namespace Automata.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class BlockAttribute : Attribute
{
    public string Name { get; }

    public string Description { get; }

    public string ProductName { get; }

    public BlockAttribute(string name, string description = "", string productName = "")
    {
        Name = name;
        Description = description;
        ProductName = productName;
    }
}
