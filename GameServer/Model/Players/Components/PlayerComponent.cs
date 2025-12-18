using GameServer.Model.Components;
using GameServer.Model.Games;

namespace GameServer.Model.Players.Components;


/// <summary>
/// Hold info about player
/// </summary>
public sealed class PlayerComponent : Component
{
    public string PlayerToken = "";

    public bool IsReady = false;
}