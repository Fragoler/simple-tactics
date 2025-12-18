using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


/// <summary>
/// Cell pattern. Configurate group of cells.
/// Provides validation of the target cell
/// </summary>
public interface IPattern
{
    public string Name { get; }
    public bool Validate(Coordinates executor, Coordinates target);
}


// Also need to dto convert
public interface IRangePattern : IPattern
{
    public double Range { get; }
}
