using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Transform;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Model.Effects;


/// <summary>
/// A Base for all client effects.
/// Use to show something for clients 
/// </summary>
public abstract class EffectArgs
{
    public required Game Game { get; init; }
    public int Duration { get; init; } = 500;

    public abstract EffectDto ToDto();
}

/// <summary>
/// Move unit to position
/// </summary>
public sealed class MoveEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Move",
        Duration = Duration,
        UnitId = Entity.Info.Id,
        From = From,
        To = To
    };
}


/// <summary>
/// Deal unit damage
/// </summary>
public sealed class DamageEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Damage",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        Amount = Amount,
    };
}


/// <summary>
/// Make unit to shoot
/// </summary>
public sealed class ShootEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity? Target { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Shoot",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        From = From,
        To = To,
        TargetUnitId = Target?.Info.Id
    };
}


/// <summary>
/// Make unit to attack
/// </summary>
public sealed class MeleeEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates From { get; init; }
    public Coordinates To { get; init; }
    public Entity? Target { get; init; } = null;
    
    public override EffectDto ToDto() => new()
    {
        Type = "Melee",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        From = From,
        To = To,
        TargetUnitId = Target?.Info.Id
    };
}


/// <summary>
/// Summon explosion
/// </summary>
public sealed class ExplosionEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Coordinates Center { get; init; }
    public double Radius { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Explosion",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        Center = Center,
        Radius = Radius
    };
}


/// <summary>
/// Kill unit
/// </summary>
public sealed class DeathEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public override EffectDto ToDto() => new()
    {
        Type = "Death",
        UnitId = Entity.Info.Id,
        Duration = Duration
    };
}

/// <summary>
/// Heal unit
/// </summary>
public sealed class HealEffectArgs : EffectArgs
{
    public Entity Entity { get; init; }
    public Entity Target { get; init; }
    public uint Amount { get; init; }
    
    public override EffectDto ToDto() => new()
    {
        Type = "Heal",
        UnitId = Entity.Info.Id,
        Duration = Duration,
        TargetUnitId = Target.Info.Id,
        Amount = Amount
    };
}
