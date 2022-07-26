public class TrainV1 : ITrainImplant
{
    private bool IsKnightTurn = false;
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
}