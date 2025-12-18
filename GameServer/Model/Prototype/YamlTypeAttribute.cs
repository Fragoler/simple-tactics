namespace GameServer.Model.Prototype;


/// <summary>
/// Mark struct as serializable for prototypes
/// </summary>
/// <param name="typeName">Associated name in yaml</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class YamlTypeAttribute(string typeName) : Attribute
{
    public string TypeName { get; } = typeName;
}