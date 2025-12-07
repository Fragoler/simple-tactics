using GameServer.Model.IoC;
using GameServer.Model.Prototype;

namespace GameServer.Services;


/// <summary>
/// Prototype service. It's needed to load prototypes before http server start
/// </summary>
public sealed class PrototypeLoaderService(
    IoCManager iocManager,
    IConfiguration conf,
    ILogger<PrototypeLoaderService> logger)
    : IHostedService
{
    private readonly PrototypeSystem _proto = iocManager.Resolve<PrototypeSystem>();
    private readonly IConfiguration _conf = conf;
    private readonly ILogger<PrototypeLoaderService> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var prototypePath = _conf.GetValue<string>("GameServer:Prototypes:BasePath");
        
        if (string.IsNullOrEmpty(prototypePath))
        {
            _logger.LogWarning("GameServer:Prototypes:BasePath not configured in appsettings.json");
            return Task.CompletedTask;
        }

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), prototypePath);
        
        if (!Directory.Exists(fullPath))
        {
            _logger.LogWarning("Prototype directory not found: {Path}", fullPath);
            return Task.CompletedTask;
        }

        try
        {
            _proto.LoadPrototypesFromDirectory(fullPath);
            
            var loadedPrototypes = _proto.GetAllPrototypeIds().ToList();
            _logger.LogInformation(
                "Loaded {Count} prototypes from {Path}: {Prototypes}", 
                loadedPrototypes.Count, 
                fullPath,
                string.Join(", ", loadedPrototypes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load prototypes from {Path}", fullPath);
            throw;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Prototype loader service stopping");
        return Task.CompletedTask;
    }
}
