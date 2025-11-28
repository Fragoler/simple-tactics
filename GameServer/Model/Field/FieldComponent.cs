namespace GameServer.Model.Field;

public class FieldComponent
{
    public FieldSize FieldSize;
}

public record struct FieldSize
{
    public uint Width;
    public uint Height;
}