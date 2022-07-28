public static class Commands
{
    public static string Build(int site, StructureType structureType, UnitType? barracksUnitType)
    {
        if (structureType == StructureType.BARRACKS)
        {
            return $"BUILD {site} {structureType}-{barracksUnitType}";
        }
        return $"BUILD {site} {structureType}";
    }

    public static string Move(int x, int y)
    {
        return $"MOVE {x} {y}";
    }

    public static string Wait()
    {
        return "WAIT";
    }
}