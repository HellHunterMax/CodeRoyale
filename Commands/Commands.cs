public static class Commands
{
    public static string Build(int site, StructureType structureType, UnitType? barracksUnitType = null)
    {
        if (structureType == StructureType.BARRACKS)
        {
            return $"BUILD {site} {structureType}-{barracksUnitType}";
        }
        return $"BUILD {site} {structureType}";
    }

    public static string Run(Site closestTower,Knight unit,Queen queen)
    {
        Point point = GetOtherSideOfSite(closestTower, new Point {X = unit.X, Y = unit.Y});
        return Commands.Move(point.X, point.Y);
    }

    public static string Move(int x, int y)
    {
        return $"MOVE {x} {y}";
    }

    public static string Wait()
    {
        return "WAIT";
    }
    private static Point GetOtherSideOfSite(Site site, Point B)
    {
        Console.Error.WriteLine($"Getting other side of Site: {site.SiteId}:");
        Point C = new Point();
        double vx = site.X - B.X;
        double vy = site.Y - B.Y;
        Console.Error.WriteLine($"Enemy Unit on Position: {B.X}, {B.Y}");
        Console.Error.WriteLine($"vx = {vx}, vy = {vy}.");
        double length = Math.Sqrt(vx*vx + vy*vy);
        Console.Error.WriteLine($"length = {length}.");
        C.X = (int)Math.Round(vx / length * (site.Radius + 30) + site.X);
        C.Y = (int)Math.Round(vy / length * (site.Radius + 30) + site.Y);
        
        Console.Error.WriteLine($"C.X = {C.X}, C.Y = {C.Y}.");
        Console.Error.WriteLine($"Finished getting other side of Site: {site.SiteId}:");
        return C;
    }
}