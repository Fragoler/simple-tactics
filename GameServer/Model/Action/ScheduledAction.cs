using GameServer.Model.Transform;

namespace GameServer.Model.Action;


public class ScheduledAction
{
    public required ulong UnitId;
    public required string ActionId;
    public required ActionTarget ActionTarget;
}

public struct ActionTarget
{
    public Coordinates? Cell;
    public ulong[]? UnitIds;
}