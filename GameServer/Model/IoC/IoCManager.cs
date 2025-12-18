using System.Reflection;

namespace GameServer.Model.IoC;


/// <summary>
/// This is the only service in ASP NET in Model
/// </summary>
public sealed class IoCManager
{
    private readonly ILoggerFactory _factory;
    private readonly Dictionary<Type, object> _services = new(); 

    public IoCManager(ILoggerFactory factory)
    {
        _factory = factory;
        _services.Add(typeof(IoCManager), this);
        
        AutoRegisterSystems();
        InitializeAll();
    }
    
    private void AutoRegisterSystems()
    {
        var systemTypes = FindDerivedTypes(Assembly.GetExecutingAssembly(), typeof(BaseSystem));
        
        foreach (var type in systemTypes)
        {
            if (type.IsAbstract)
                continue;
            
            Register(type);
        }
    }

    public T Resolve<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        throw new InvalidOperationException($"Service {typeof(T).Name} is not registered");
    }
    
    private void Register(Type type)
    {
        if (_services.ContainsKey(type))
            throw new InvalidOperationException($"Service {type.Name} is already registered");

        if (Activator.CreateInstance(type) is BaseSystem system)
        {
            _services.Add(type, system);
        }
        else
        {
            throw new InvalidOperationException($"Error creating system type: {type.Name}");
        }
    }
    
    private void InitializeAll()
    {
        foreach (var service in _services.Values)
            InjectDependencies(service);
        
        foreach (var service in _services.Values)
            if (service is BaseSystem system)
                system.Initialize();
    }

    public void InjectDependencies(object target)
    {
        var type = target.GetType();


        if (target is ILoggerUser logUser)
        {
            var loggerProperty = typeof(ILoggerUser).GetProperty("Logger",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (loggerProperty != null && loggerProperty.CanWrite)
            {
                loggerProperty.SetValue(logUser, _factory.CreateLogger(type));
            }
        }


        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<DependencyAttribute>() is null)
                continue;

            if (_services.TryGetValue(field.FieldType, out var dependency))
            {
                field.SetValue(target, dependency);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Dependency {field.FieldType.Name} not found for {type.Name}");
            }
        }
        
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<DependencyAttribute>() is null)
                continue;
                
            if (!property.CanWrite)
                continue;

            if (_services.TryGetValue(property.PropertyType, out var dependency))
            {
                property.SetValue(target, dependency);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Dependency {property.PropertyType.Name} not found for {type.Name}");
            }
        }
    }

    private static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType);
    }
}
