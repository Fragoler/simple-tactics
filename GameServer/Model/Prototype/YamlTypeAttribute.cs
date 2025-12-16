namespace GameServer.Model.Prototype;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class YamlTypeAttribute(string typeName) : Attribute
{
    public string TypeName { get; } = typeName;
}