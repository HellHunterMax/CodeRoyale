public enum UnitType
{
    QUEEN = -1,
    KNIGHT = 0,
    ARCHER = 1,
    GIANT = 2
}

public static class UnitTypeMapper 
{
    public static UnitType MapToUnitType(this int number) => number switch
    {
        -1 => UnitType.QUEEN,
        0 => UnitType.KNIGHT,
        1 => UnitType.ARCHER,
        2 => UnitType.GIANT,
        _ => throw new ArgumentOutOfRangeException($"Unknown UnitType for number: {number}"),
    };

    public static int MapToInt(this UnitType unitType) => unitType switch
    {
        UnitType.QUEEN => -1,
        UnitType.KNIGHT => 0,
        UnitType.ARCHER => 1,
        UnitType.GIANT => 2,
        _ => throw new ArgumentOutOfRangeException($"Unknown number for UnitType: {unitType}"),
    };
}