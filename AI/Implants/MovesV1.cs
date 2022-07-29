public class MovesV1 : IMoveImplant
{
    public string GetMoveCommand(Field field, List<Site> sites, Queen queen)
    {
        var (closestNonHostileSite, distance) = GetClosestNonHostileSite(field, sites, queen);
        var areThereEnoughMines = CountMines(sites) >1;
        var areThereEnoughBarracks = CountBarracks(sites) > 1;
        var areThereEnoughTowers = CountTowers(sites) > 1;
        var touchedSite = sites.FirstOrDefault(x=> x.SiteId == queen.TouchedSite);

        if  (closestNonHostileSite == null || (areThereEnoughTowers && areThereEnoughBarracks && areThereEnoughMines))
        {
            // Check for closest site maxGold if low then build archer
            // then go to middle build mine max upgrade
            // go vertical build tower upgrade.
            // go vertical build mine & upgrade
            // go vertical build barracks
            // go Horizontal buildTower
            // TODO if no gold in mine dont go and guild mine there.
            return Commands.Wait();
        }

        var buildTouchedSiteCommand = GetBuildTouchedSiteCommand(touchedSite);

        if (buildTouchedSiteCommand != null)
        {
            return buildTouchedSiteCommand;
        }

        if (distance > Queen.Radius)
        {
            return Commands.Move(closestNonHostileSite.X, closestNonHostileSite.Y);
        }

        if (!areThereEnoughMines)
        {
            return Commands.Build(closestNonHostileSite.SiteId, StructureType.MINE, null);
        }

        if (!areThereEnoughBarracks )
        {
            var barracksType = GetBarracksTypeToBuild(sites);
            return Commands.Build(closestNonHostileSite.SiteId, StructureType.BARRACKS, barracksType);
        }

        if (!areThereEnoughTowers)
        {
            return Commands.Build(closestNonHostileSite.SiteId, StructureType.TOWER, null);
        }
        
            return Commands.Wait();
    }

    private string? GetBuildTouchedSiteCommand(Site? touchedSite)
    {
        if (touchedSite != null && touchedSite.Structure.Type == StructureType.MINE)
        {
            var mine = (Mine)touchedSite.Structure;
            if (mine.IncomeRate != touchedSite.MaxMineSize)
            {
                return Commands.Build(touchedSite.SiteId, StructureType.MINE, null);
            }
        }
        if (touchedSite != null && touchedSite.Structure.Type == StructureType.TOWER)
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

    private int CountMines(List<Site> sites)
    {
        return sites.Count(x => x.Owner == Owner.Friendly && x.Structure.Type == StructureType.MINE);
    }

    private int CountTowers(List<Site> sites)
    {
        return sites.Count(x => x.Owner == Owner.Friendly && x.Structure.Type == StructureType.TOWER);
    }

    private int CountBarracks(List<Site> sites)
    {
        return sites.Count(x => x.Owner == Owner.Friendly && x.Structure.Type == StructureType.BARRACKS);
    }

    private UnitType GetBarracksTypeToBuild(List<Site> sites)
    {

        var knight = 0;
        var archer = 0;

        foreach (var site in sites)
        {
            if (site.Owner != Owner.Friendly || site.Structure.Type != StructureType.BARRACKS) //Friendly
            {
                continue;
            }
            switch (((Barracks)site.Structure).UnitType)
            {
                case UnitType.ARCHER :
                archer++;
                break;
                case UnitType.KNIGHT :
                knight++;
                break;
                default:
                break;
            }
        }

        return archer <= knight ? UnitType.ARCHER : UnitType.KNIGHT;
    }
    private (Site?, int) GetClosestNonHostileSite(Field field, List<Site> sites, Queen queen)
    {
        Site? closestSite = null;
        var closestDistance = int.MaxValue;
        foreach (var site in sites)
        {
            if (site.Owner != Owner.Neutral) //Only go to sites that are neutral
            {
                continue;
            }
            var distance = GetDistance(queen, site);
            closestSite = distance < closestDistance ? site : closestSite;
            closestDistance = distance < closestDistance ? distance : closestDistance;
        }
        return (closestSite, closestDistance);
    }

    private int GetDistance(Queen queen, Site site)
    {
        var xDistance = Math.Abs(queen.X - site.X) - site.Radius;
        var yDistance = Math.Abs(queen.Y - site.Y) - site.Radius;
        return (xDistance + yDistance);
    }
}