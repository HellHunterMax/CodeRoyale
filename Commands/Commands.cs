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

    public static string Run(Site site, IFieldItem fieldItem)
    {
        Point point = GetOtherSideOfSite(site, new Point {X = fieldItem.X, Y = fieldItem.Y});
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
    public static string? BuildTouchedSiteCommand(Site? touchedSite)
    {
        if (touchedSite != null && touchedSite.Structure.Type == StructureType.MINE)
        {
            var mine = (Mine)touchedSite.Structure;
            if (mine.IncomeRate != touchedSite.MaxMineSize) //TODO && !unitsInRange
            {
                return Commands.Build(touchedSite.SiteId, StructureType.MINE, null);
            }
        }
        if (touchedSite != null && touchedSite.Structure.Type == StructureType.TOWER) //TODO && !unitsInRange or range is > 400
        {
            var tower = (Tower)touchedSite.Structure;
            Console.Error.WriteLine($"tower maxRadius == {tower.AttackRadius}");
            if (tower.AttackRadius <= 500)
            {
                return Commands.Build(touchedSite.SiteId, StructureType.TOWER, null);
            }
        }
        return null;
    }
    public static string? BuildTowerCommand(Queen queen, Site[] towers)
    {
        Site? site = null;
        if (towers[0].Structure.Type != StructureType.TOWER)
        {
            if (towers[1].Structure.Type == StructureType.TOWER)
            {
                site = towers[0];
            }
            else
            {
                var distance0 = queen.GetDistance(towers[0]);
                var distance1 = queen.GetDistance(towers[1]);
                site = distance0 < distance1 ? towers[0] : towers[1];
            }
        }
        else if (towers[1].Structure.Type != StructureType.TOWER)
        {
            site = towers[1];
        }
        if (site == null)
        {
            return null;
        }
        return Commands.Build(site.SiteId, StructureType.TOWER);
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
        C.X = (int)Math.Round(vx / length * (site.Radius + 60) + site.X);
        C.Y = (int)Math.Round(vy / length * (site.Radius + 60) + site.Y);
        
        Console.Error.WriteLine($"C.X = {C.X}, C.Y = {C.Y}.");
        Console.Error.WriteLine($"Finished getting other side of Site: {site.SiteId}:");
        return C;
    }
}