namespace GameServer.Model.EventBus;



/// <summary>
/// An event related to component lifecycle
/// These events execute only one time for 
/// </summary>
public interface IComponentLifecycleEvent : IBaseEvent
{
    public Type CompType { get; }
}

public abstract class ComponentLifecycleEvent : BaseEvent, IComponentLifecycleEvent
{
    public required Type CompType { get; init; }
}