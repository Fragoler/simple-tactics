using GameServer.Model.Action.Components;

namespace GameServer.Presenter.Socket.DTO;


public class ActionDto
{
    public required string Id { get; set; }
    
    public required string Name { get; set; }
    public required string Icon { get; set; }
    
    public required string TargetType { get; set; }
    public TargetFilterDto TargetFilter { get; set; }

    public required HighlightedLayerDto[] HighlightLayers { get; set; }
}


public struct TargetFilterDto
{
    public TargetFilterDto(TargetFilter filter)
    {
        Pattern = filter.Pattern.ToString();
        Range = filter.Range;
        RequiredFreeSpace = filter.RequiredFreeSpace;
        MaxTargets = filter.MaxTargets;
        RequiredAlly = filter.RequiredAlly;
        RequiredEnemy = filter.RequiredEnemy;
    }

    public string Pattern { get; set; } = "";
    public double Range { get; set; } = 1.5;
    public bool RequiredEnemy { get; set; } = false;
    public bool RequiredAlly { get; set; } =  false;
    public bool RequiredFreeSpace { get; set; } = false;
    public uint MaxTargets { get; set; } = 1;
}

public struct HighlightedLayerDto
{
    public HighlightedLayerDto(HighlightedLayer layer)
    {
        Range = layer.Range;
        
        Type = layer.Type.ToString();
        Pattern = layer.Pattern.ToString();
        Relative = layer.Relative.ToString();
        Visibility = layer.Visibility.ToString();
    }
    
    public string Type { get ; set; }
    public string Pattern { get; set; }
    public string Relative { get; set; }
    public string Visibility { get; set; }
    
    public double? Range  { get; set; }
}