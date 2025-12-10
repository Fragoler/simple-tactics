using System.Text.Json.Serialization;
using GameServer.Model.Transform;

namespace GameServer.Presenter.Socket.DTO;


public struct ScheduledActionDto
{
    public ScheduledActionDto()
    {
    }

    [JsonPropertyName("unitId")] public ulong UnitId { get; set; } = 0;

    [JsonPropertyName("actionId")] public string ActionId { get; set; } = "";

    [JsonPropertyName("target")] public Coordinates? Target { get; set; } = null;
}