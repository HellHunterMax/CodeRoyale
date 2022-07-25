public class Ai
{
    private bool IsKnightTurn = false;

    public string GetMoveCommand(Field field, List<Site> sites, Queen queen)
    {
        var (closestNonHostileSite, distance) = GetClosestNonHostileSite(field, sites, queen);
        var areThereAnoughBarracks = CountBarracks(sites) > 1;
        var areThereEnoughTowers = CountTowers(sites) > 1;

        if  (closestNonHostileSite == null || (areThereEnoughTowers && areThereAnoughBarracks))
        {
            var site = sites.First(x=> x.SiteId == queen.TouchedSite);
            if( site != null && site.StructureType == StructureType.TOWER)
            {
                return $"BUILD {queen.TouchedSite} {StructureType.TOWER}";
            }
            //TODO Run From Enemy!
            //Find Perfect location to hide.
            return "WAIT";
        }

        if (distance > Queen.Radius)
        {
            return $"MOVE {closestNonHostileSite.X} {closestNonHostileSite.Y}";
        }

        if (areThereAnoughBarracks )
        {
            return $"BUILD {closestNonHostileSite.SiteId} {StructureType.TOWER}";
        }


        var barracksType = GetBarracksTypeToBuild(sites);
        return $"BUILD {closestNonHostileSite.SiteId} {StructureType.BARRACKS}-{barracksType}";
    }

    private int CountTowers(List<Site> sites)
    {
        return sites.Count(x => x.Owner == Owner.Friendly && x.StructureType == StructureType.TOWER);
    }

    private int CountBarracks(List<Site> sites)
    {
        return sites.Count(x => x.Owner == Owner.Friendly && x.StructureType == StructureType.BARRACKS);
    }

    private UnitType GetBarracksTypeToBuild(List<Site> sites)
    {

        var knight = 0;
        var archer = 0;

        foreach (var site in sites)
        {
            if (site.Owner != Owner.Friendly) //Friendly
            {
                continue;
            }
            if (site.Barracks != null)
            {
                switch (site.Barracks.UnitType)
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
        }

        return archer <= knight ? UnitType.ARCHER : UnitType.KNIGHT;
    }

    public string GetTrainCommand(Field field, List<Site> sites, Queen queen)
    {
        var command = "TRAIN";
        var site = GetSiteForTraining(field, sites, queen);

        while (site != null)
        {
            command += $" {site.SiteId}";
            site = GetSiteForTraining(field, sites, queen);
        }
        return command;
    }

    private Site? GetSiteForTraining(Field field, List<Site> sites, Queen queen)
    {
        List<Site> availableSites = GetAvailableSites(sites);
        if (!availableSites.Any())
        {
            return null;
        }
        var canBuildKnight = availableSites.Any(x => x.Barracks!.UnitType == UnitType.KNIGHT);
        var canBuildArcher = availableSites.Any(x => x.Barracks!.UnitType == UnitType.ARCHER);

        var AreThereEnoughArchers = GetNumberOfArchers(field) > 1;
        var gold = IsKnightTurn? 80 : 100;
        UnitType unitType;

        if (canBuildKnight)
        {
            gold = 80;
            unitType = UnitType.KNIGHT;
            if (!IsKnightTurn && !AreThereEnoughArchers)
            {
                if (canBuildArcher)
                {
                    gold = 100;
                    unitType = UnitType.ARCHER;
                }
            }
        }
        else if (canBuildArcher)
        {
            gold = 100;
            unitType = UnitType.ARCHER;
        }
        else
        {
            return null;
        }

        if (queen.Gold < gold)
        {
            return null;
        }
        Site site;
        if (unitType == UnitType.ARCHER)
        {
            IsKnightTurn = true;
            site = availableSites.First(x => x.Barracks!.UnitType == UnitType.ARCHER);
        }
        else
        {
            IsKnightTurn = false;
            site = availableSites.First(x => x.Barracks!.UnitType == UnitType.KNIGHT);
        }
        site.Barracks!.IsTraining = true;
        queen.Gold -= gold;
        return site;
    }

    private int GetNumberOfArchers(Field field)
    {
        return field.FriendlyArchers.Count;
    }

    private List<Site> GetAvailableSites(List<Site> sites)
    {
        var availableSites = new List<Site>();

        foreach (var site in sites)
        {
            if  (site.Owner != Owner.Friendly || 
                site.Barracks == null || 
                site.Barracks.IsTraining)
            {
                continue;
            }
            availableSites.Add(site);
        }
        return availableSites;
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
        return xDistance + yDistance;
    }
}