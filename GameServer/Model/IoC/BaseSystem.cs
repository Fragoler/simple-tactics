namespace GameServer.Model.IoC;


public abstract class BaseSystem : ILoggerUser
{
    protected IoCManager IoC { get; private set; } = null!;

    public ILogger Logger { get; set; } = null!;

    public virtual void Initialize()
    {
        Logger.LogInformation("{Name} initialized", GetType().Name);
    }
}