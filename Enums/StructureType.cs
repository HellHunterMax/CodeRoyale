public enum StructureType
{
    EMPTY = -1,
    MINE = 0,
    TOWER = 1,
    BARRACKS= 2
}

public static class StructureTypeMapper
{
    public static int ToInt(this StructureType structure) => structure switch
    {
        StructureType.EMPTY => -1,
        StructureType.MINE => 0,
        StructureType.TOWER => 1,
        StructureType.BARRACKS => 2,
        _ => throw new ArgumentOutOfRangeException($"Unknown Number for StructureType: {structure}"),
    };

    public static StructureType ToStructureType(this int number) => number switch
    {
        -1 => StructureType.EMPTY,
        0 => StructureType.MINE,
        1 => StructureType.TOWER,
        2 => StructureType.BARRACKS,
        _ => throw new ArgumentOutOfRangeException($"Unknown StructureType for number: {number}"),
    };
}