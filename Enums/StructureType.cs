public enum StructureType
{
    SITE = -1,
    TOWER = 1,
    BARRACKS= 2
}

public static class StructureTypeMapper
{
    public static int MapToint(this StructureType structure) => structure switch
    {
        StructureType.SITE => -1,
        StructureType.TOWER => 1,
        StructureType.BARRACKS => 2,
        _ => throw new ArgumentOutOfRangeException($"Unknown Number for StructureType: {structure}"),
    };

    public static StructureType MapToStructureType(this int number) => number switch
    {
        -1 => StructureType.SITE,
        1 => StructureType.TOWER,
        2 => StructureType.BARRACKS,
        _ => throw new ArgumentOutOfRangeException($"Unknown StructureType for number: {number}"),
    };
}