namespace GameServer.Model.IoC;


public abstract class BaseSystem
{
    protected ILogger Logger { get; private set; } = null!;

    public virtual void Initialize()
    {
        Logger.LogInformation("{Name} initialized", GetType().Name);
    }
}