using GameServer.Model.Transform;

namespace GameServer.Presenter.Socket.DTO;


public abstract record EffectDto
{
    public required string Type { get; init; }
    public ulong UnitId { get; init; }
    public int Duration { get; init; } = 1000;
}

public record MoveEffectDto : EffectDto
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
}

public record DamageEffectDto : EffectDto
{
    public uint Amount { get; init; }
}

public record ShootEffectDto : EffectDto
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public ulong? TargetUnitId { get; init; }
}

public record MeleeEffectDto : EffectDto
{
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public ulong TargetUnitId { get; init; }
}

public record ExplosionEffectDto : EffectDto
{
    public Coordinates Center { get; init; }
    public double Radius { get; init; }
}

public record DeathEffectDto : EffectDto
{
}

public record HealEffectDto : EffectDto
{
    public ulong TargetUnitId { get; init; }
    public uint Amount { get; init; }
}