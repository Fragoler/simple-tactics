using GameServer.Model.Transform;

namespace GameServer.Presenter.Socket.DTO;


public sealed record EffectDto
{
    public required string Type { get; init; }
    public required int Duration { get; init; }

    public ulong? UnitId { get; init; } = null;
    public Coordinates? From { get; init; } = null;
    public Coordinates? To { get; init; } = null;
    public ulong? TargetUnitId { get; init; } = null;
    public uint? Amount { get; init; } = null;
    public Coordinates? Center { get; init; } = null;
    public double? Radius { get; init; } = null!;
}