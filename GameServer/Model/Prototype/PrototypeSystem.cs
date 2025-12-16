using System.Reflection;
using Force.DeepCloner;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GameServer.Model.Prototype;

public sealed class PrototypeSystem : BaseSystem
{
    [Dependency] private EntitySystem _entity = null!;
    [Dependency] private ComponentSystem _comp = null!;
    
    private readonly Dictionary<string, EntityPrototype> _prototypes = [];
    private IDeserializer _deserializer = null!;
    
    private const string CompPostfix = "Component";

    public override void Initialize()
    {
        _deserializer = BuildDeserializer();
    }

    private IDeserializer BuildDeserializer()
    {
        var builder = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties();
        
        RegisterComponentTypes(builder);
        RegisterYamlTypes(builder);

        return builder.Build();
    }

    private void RegisterComponentTypes(DeserializerBuilder builder)
    {
        var componentTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && 
                   typeof(Component).IsAssignableFrom(t));

        foreach (var type in componentTypes)
        {
            if (!type.Name.EndsWith(CompPostfix))
                continue;
            
            var shortName = type.Name[..^CompPostfix.Length];
            var tagName = new TagName($"!type:{shortName}");
            
            builder.WithTagMapping(tagName, type);
            
            Logger.LogDebug("Registered component tag {Tag} -> {Type}", tagName, type.Name);
        }
    }

    private void RegisterYamlTypes(DeserializerBuilder builder)
    {
        var yamlTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<YamlTypeAttribute>() != null);

        foreach (var type in yamlTypes)
        {
            var attr = type.GetCustomAttribute<YamlTypeAttribute>()!;
            var tagName = new TagName($"!type:{attr.TypeName}");
            
            builder.WithTagMapping(tagName, type);
            
            Logger.LogDebug("Registered YAML tag {Tag} -> {Type}", tagName, type.Name);
        }
    }

    public void LoadPrototypesFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Prototype directory not found: {directoryPath}");

        var yamlFiles = Directory.GetFiles(directoryPath, "*.yml", SearchOption.AllDirectories)
            .Concat(Directory.GetFiles(directoryPath, "*.yaml", SearchOption.AllDirectories));

        foreach (var filePath in yamlFiles)
        {
            try
            {
                LoadPrototypesFromFile(filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load prototypes from {filePath}", ex);
            }
        }
    }

    public void LoadPrototypesFromFile(string filePath)
    {
        var yamlContent = File.ReadAllText(filePath);
        LoadPrototypes(yamlContent);
    }
    
    public void LoadPrototypes(string yamlContent)  
    {
        Logger.LogDebug("Loading prototypes from YAML (length: {Length} chars)", yamlContent.Length);
    
        List<EntityPrototype>? prototypes;
    
        try
        {
            prototypes = _deserializer.Deserialize<List<EntityPrototype>>(yamlContent);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to deserialize YAML");
            throw;
        }

        Logger.LogDebug("Deserialized {Count} prototypes", prototypes.Count);

        foreach (var prototype in prototypes)
        {
            if (!_prototypes.TryAdd(prototype.Id, prototype))
                throw new InvalidOperationException($"Prototype {prototype.Id} already exists");

            Logger.LogDebug("Successfully loaded prototype {Id}", prototype.Id);
        }
    }


    public Entity SpawnPrototype(string prototypeId, Game game)
    {
        if (!_prototypes.TryGetValue(prototypeId, out var prototype))
            throw new InvalidOperationException($"Prototype {prototypeId} not found");

        var entity = _entity.CreateEntity(game);
        
        foreach (var cloned in prototype.Components.Select(component => component.DeepClone()))
        {
            cloned.Owner = entity;
            _comp.AddComponent(entity, cloned);
        }

        return entity;
    }
    
    public bool HasPrototype(string prototypeId)
    {
        return _prototypes.ContainsKey(prototypeId);
    }

    public EntityPrototype? GetPrototype(string prototypeId)
    {
        return _prototypes.GetValueOrDefault(prototypeId);
    }

    public IEnumerable<string> GetAllPrototypeIds()
    {
        return _prototypes.Keys;
    }
}
