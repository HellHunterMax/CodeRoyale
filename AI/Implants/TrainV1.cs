public class TrainV1 : ITrainImplant
{
    private bool _isKnightTurn = false;
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
            var availableSites = GetAvailableBarracks(sites);
            if (!availableSites.Any())
            {
                return null;
            }
            var canBuildKnight = availableSites.Any(x => GetBarrack(x)?.UnitType == UnitType.KNIGHT);

            var gold = 0;
            UnitType? unitType = null;

            if (canBuildKnight && queen.Gold > Knight.GoldCost)
            {
                gold = Knight.GoldCost;
                unitType = UnitType.KNIGHT;
            }

            if (queen.Gold < gold || unitType == null)
            {
                return null;
            }

            Site site;

            site = availableSites.First(x => GetBarrack(x)?.UnitType == UnitType.KNIGHT);
            GetBarrack(site)!.IsTraining = true;
            queen.Gold -= gold;
            return site;
        }

    private Barracks? GetBarrack(Site site)
    {
        if (site.Structure.Type != StructureType.BARRACKS)
        {
            return null;
        }
        return (Barracks)site.Structure;
    }

    private List<Site> GetAvailableBarracks(List<Site> sites)
    {
        var availableSites = new List<Site>();

        foreach (var site in sites)
        {
            if  (site.Owner != Owner.Friendly ||
                GetBarrack(site)?.IsTraining != false)
            {
                continue;
            }
            availableSites.Add(site);
        }
        return availableSites;
    }
}