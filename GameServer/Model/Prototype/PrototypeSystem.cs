using System.Reflection;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GameServer.Model.Prototype;

public sealed class PrototypeSystem : BaseSystem
{
    [Dependency] private EntitySystem _entity = null!;
    [Dependency] private ComponentSystem _comp = null!;
    
    private readonly Dictionary<string, EntityPrototype> _prototypes = []; // Loaded protypes
    private readonly Dictionary<string, Type> _compTypeCache = []; // Cache TypeName of components with their shortnames
    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(PascalCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();
    
    
    private const string CompPostfix = "Component";


    public override void Initialize()
    {
        BuildComponentTypeCache();
    }

    private void BuildComponentTypeCache()
    {
        var componentTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(Component).IsAssignableFrom(t));

        foreach (var type in componentTypes)
        {
            if (!type.Name.EndsWith(CompPostfix))
                continue;
            
            var shortName = type.Name[..^CompPostfix.Length];
            Logger.LogDebug("Cached type {CompType} as a {shortName}", type.Name, shortName);
            _compTypeCache.Add(shortName, type);
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
        var prototypes = _deserializer.Deserialize<List<EntityPrototype>>(yamlContent);
        
        if (prototypes.Count == 0)
            return;

        foreach (var prototype in prototypes)
        {
            Logger.LogDebug("Loading prototype {PrototypeId}", prototype.Id);
            if (_prototypes.ContainsKey(prototype.Id))
                throw new InvalidOperationException($"Prototype {prototype.Id} already exists");
            
            ResolvePrototypeComponents(prototype);
            
            _prototypes.Add(prototype.Id, prototype);
        }
    }

    private void ResolvePrototypeComponents(EntityPrototype prototype)
    {
        prototype.ResolvedComponents.Clear();
        
        foreach (var componentData in prototype.Components)
        {
            var componentType = ResolveComponentType(componentData.Type);
            if (componentType == null)
                throw new InvalidOperationException(
                    $"Component type '{componentData.Type}' not found in prototype '{prototype.Id}'");

            prototype.ResolvedComponents.Add(new ResolvedComponentData
            {
                ComponentType = componentType,
                Data = componentData.Data
            });
        }
    }

    public Entity SpawnPrototype(string prototypeId, Game game)
    {
        if (!_prototypes.TryGetValue(prototypeId, out var prototype))
            throw new InvalidOperationException($"Prototype {prototypeId} not found");

        var entity = _entity.CreateEntity(game);

        foreach (var resolvedComp in prototype.ResolvedComponents)
            _comp.AddComponent(entity, resolvedComp.ComponentType, resolvedComp.Data);

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
    
    private Type? ResolveComponentType(string shortName)
    {
        return _compTypeCache.GetValueOrDefault(shortName);
    }
}
