using GameServer.Model.Components;
using GameServer.Model.Contexts;
using GameServer.Model.IoC;

namespace GameServer.Model.Systems;


public abstract class BaseSystem
{
    public virtual void Initialize()
    {
        Console.WriteLine($"{GetType().Name} initialized");
    }
}