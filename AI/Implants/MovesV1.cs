public class MovesV1 : IMoveImplant
{
    public string GetMoveCommand(Field field, List<Site> sites, Queen queen)
    {
        var (closestNonHostileSite, distance) = GetClosestNonHostileSite(field, sites, queen);
        var areThereAnoughBarracks = CountBarracks(sites) > 1;
        var areThereEnoughTowers = CountTowers(sites) > 1;

        if  (closestNonHostileSite == null || (areThereEnoughTowers && areThereAnoughBarracks))
        {
            var site = sites.FirstOrDefault(x=> x.SiteId == queen.TouchedSite);
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