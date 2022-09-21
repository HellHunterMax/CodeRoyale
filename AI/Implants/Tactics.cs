public class Tactics
{
    public bool _IsBarracksBuilt = false;
    public int _MyMines = 0;
    public int _MyBarracks = 0;
    public int _MyTowers = 0;
    public List<Site> _BarracksSites;
    public Site[] _TowerLocations;
    public readonly MySide _MySide;
    public readonly Queen _Queen;
    public readonly List<Site> _Sites;
    public readonly Field _Field;
    internal bool _AreThereEnoughMines;
    internal bool _AreThereEnoughBarracks;
    internal bool _AreThereEnoughTowers;

    public Tactics(Game game)
    {
        _Field = game.Field;
        _Sites = game.Sites;
        _Queen = game.Queen;
        IOrderedEnumerable<KeyValuePair<Site, int>> SiteDistances = game.Queen.GetSitesAndDistance(game.Sites);
        var isMySideLeft = game.Queen.X < Field.MaxWidth/2 ? true : false;
        _MySide = isMySideLeft ? new MySide(0, 1920/2) : new MySide(1920/2, 1920);
        _TowerLocations = GetTowerLocations(game.Sites, isMySideLeft);
        _BarracksSites = GetBestBarracksSites(SiteDistances);

    Console.Error.WriteLine($"GoodBarracks Places: ");
        foreach (var item in _BarracksSites)
        {
            Console.Error.Write($"{item.SiteId}, ");
        }
        Console.Error.WriteLine();
    }
    public void Update(Game game)
    {
        _MyMines = CountMines(game.Sites);
        _MyBarracks = CountBarracks(game.Sites);
        _MyTowers = CountTowers(game.Sites);
        _AreThereEnoughMines = _MyMines > 5;
        _AreThereEnoughBarracks = _MyBarracks > 0;
        _AreThereEnoughTowers = _MyTowers > 1;
    }
    private Site[] GetTowerLocations(List<Site> sites, bool isMySideLeft)
    {
        Console.Error.WriteLine($"Getting all Tower Locations:");
        var middle = Field.MaxWidth / 2;
        Site[]? bestSiteDuo = null;
        int bestSiteDuoMultiplier = 0;
        List<Site[]> towerDuos = GetTowerDuos(sites,  isMySideLeft);

        foreach (var duo in towerDuos)
        {
            var duoMultiplier = isMySideLeft ? duo[0].X + duo[1].X : (middle - (duo[0].X - middle)) + (middle - (duo[1].X - middle));
            Console.Error.WriteLine($"TowerDuo :");
            Console.Error.WriteLine($"Site[0] : {duo[0].SiteId}, Site[1] : {duo[1].SiteId}. Multiplier = {duoMultiplier}");
            if (bestSiteDuo == null)
            {
                bestSiteDuo = duo;
                bestSiteDuoMultiplier = duoMultiplier;
                continue;
            }
            var bestSiteDuoMaxMineSize = bestSiteDuo[0].MaxMineSize + bestSiteDuo[1].MaxMineSize;
            var bestSiteDuoRemainingGold = bestSiteDuo[0].RemainingGold + bestSiteDuo[1].RemainingGold;
            var duoMaxMineSize = duo[0].MaxMineSize + duo[1].MaxMineSize;
            var duoRemainingGold = duo[0].RemainingGold + duo[1].RemainingGold;
            
            if (duoMaxMineSize > bestSiteDuoMaxMineSize && duoRemainingGold > bestSiteDuoRemainingGold && duoMultiplier > bestSiteDuoMultiplier)
            {
                bestSiteDuo = duo;
                bestSiteDuoMultiplier = duoMultiplier;
            }
        }
        if (bestSiteDuo == null)
        {
            throw new Exception("No SiteDuos found");
        }
        Console.Error.WriteLine($"Best Sites for Towers = {bestSiteDuo[0].SiteId}, {bestSiteDuo[1].SiteId}");
        return bestSiteDuo;
    }
    private List<Site[]> GetTowerDuos(List<Site> sites, bool isMySideLeft)
    {
        int maxTowerRange = 300;
        List<Site[]> siteDuos = new List<Site[]>();
        foreach (var siteA in sites)
        {
            if (!_MySide.IsFieldItemOnMySide(siteA) || siteA.Equals(_MyBarracks))
            {
                continue;
            }
            foreach (var siteB in sites)
            {
                if (siteA.Equals(siteB) ||
                    !_MySide.IsFieldItemOnMySide(siteB) || 
                    siteA.GetDistanceTo(siteB) > maxTowerRange ||
                    Math.Abs(siteA.Y - siteB.Y) > 100)
                {
                    continue;
                }
                Site[] siteDuo = new Site[] {siteA, siteB};
                if (siteDuos.Any(s => IsSiteDuo(s, siteA, siteB)))
                {
                    continue;
                }
                siteDuos.Add(siteDuo);
            }
        }
        return siteDuos;
    }
    private static bool IsSiteDuo(Site[] siteDuo, Site siteA, Site siteB)
    {
        if (siteDuo[0] == siteA)
        {
            if (siteDuo[1] == siteB)
            {
                return true;
            }
        }
        else if (siteDuo[1] == siteA)
        {
            if (siteDuo[0] == siteB)
            {
                return true;
            }
        }
        return false;
    }
    private List<Site> GetBestBarracksSites(IOrderedEnumerable<KeyValuePair<Site, int>> siteDistances)
    {
        List<Site> bestSites = new List<Site>();
        foreach (var site in siteDistances)
        {
            if (!_MySide.IsFieldItemOnMySide(site.Key) || site.Key == _TowerLocations[0] || site.Key == _TowerLocations[1])
            {
                continue;
            }
            if (site.Key.MaxMineSize < 2)
            {
                bestSites.Add(site.Key);
            }
        }
        return bestSites;
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
}