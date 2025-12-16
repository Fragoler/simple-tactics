namespace GameServer.Model.EventBus;

public interface IComponentLifeCircleEvent : IBaseEvent
{
    public Type CompType { get; }
}

public abstract class ComponentLifeCircleEvent : BaseEvent, IComponentLifeCircleEvent
{
    public required Type CompType { get; init; }
}