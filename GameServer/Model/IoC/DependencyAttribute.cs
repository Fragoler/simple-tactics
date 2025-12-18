namespace GameServer.Model.IoC;


/// <summary>
/// Mark field to injection
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DependencyAttribute : Attribute { }

