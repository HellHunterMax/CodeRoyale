public class QueenImplantV2 : IQueenImplant
{
    //TODO dont go into enemy Towers if there are no knights in range.
    //TODO build more Towers when one of Towerlocations is taken or there  is nearby empty site.
    //TODO fix bug when on Enemy Building
    //TODO Run away sooner
    //TODO Upgrade towers to fullest, start with building the one closest to the frontline
    //TODO REFACTOR TO STATES!
    private bool _IsInitialSetup = true;
    private bool _IsBarracksBuilt = false;
    private int _MyMines = 0;
    private int _MyBarracks = 0;
    private int _MyTowers = 0;
    private List<Site> _BarracksSites;
    private Site[] _TowerLocations;
    private readonly int[] _MySide;

    public QueenImplantV2(Field field, List<Site> sites, Queen queen)
    {
        IOrderedEnumerable<KeyValuePair<Site, int>> SiteDistances = GetSitesAndDistance(sites, queen);
        var isMySideLeft = queen.X < Field.MaxWidth/2 ? true : false;
        _MySide = isMySideLeft ? new[] {0, 1920/2} : new[] {1920/2, 1920};
        _TowerLocations = GetTowerLocations(sites, isMySideLeft);
        _BarracksSites = GetBestBarracksSites(SiteDistances);

    Console.Error.WriteLine($"GoodBarracks Places: ");
        foreach (var item in _BarracksSites)
        {
            Console.Error.Write($"{item.SiteId}, ");
        }
        Console.Error.WriteLine();
    }

    private List<Site> GetBestBarracksSites(IOrderedEnumerable<KeyValuePair<Site, int>> siteDistances)
    {
        List<Site> bestSites = new List<Site>();
        foreach (var site in siteDistances)
        {
            if (!IsFieldItemOnMySide(site.Key) || site.Key == _TowerLocations[0] || site.Key == _TowerLocations[1])
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

    public string GetQueenCommand(Field field, List<Site> sites, Queen queen)
    {
        _MyMines = CountMines(sites);
        _MyBarracks = CountBarracks(sites);
        _MyTowers = CountTowers(sites);
        _IsBarracksBuilt = _BarracksSites.Any(s => s.Structure.Type == StructureType.BARRACKS);
        
        var areThereEnoughMines = _MyMines > 5;
        var areThereEnoughBarracks = _MyBarracks > 0;
        var areThereEnoughTowers = _MyTowers > 1;

        var SiteDistances = GetSitesAndDistance(sites, queen);

        var (closestNonHostileSite, closestSiteDistance) = GetClosestNonHostileSite(field, sites, queen);
        var touchedSite = sites.FirstOrDefault(x=> x.SiteId == queen.TouchedSite);

        if (_IsInitialSetup && IsEnemyTraining(sites))
        {
            if (!_IsBarracksBuilt)
            {
                var closestBarracksSite = SiteDistances.First(x => _BarracksSites.Any(s => s.SiteId ==x.Key.SiteId)).Key;
                Console.Error.WriteLine($"Building Barracks on Site {closestBarracksSite.SiteId}!");
                return Commands.Build(closestBarracksSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (touchedSite != null)
            {
                var command = GetBuildTouchedSiteCommand(touchedSite);
                if (command != null)
                {
                    return command;
                }
            }
            if (!areThereEnoughTowers)
            {
                string? buildTowerCommand = GetBuildTowerCommand(queen);
                if (buildTowerCommand != null)
                {
                Console.Error.WriteLine($"Building Tower! {buildTowerCommand}");
                    return buildTowerCommand;
                }
            }
        }

        if (field.CountUnitsOf(UnitType.KNIGHT, Owner.Enemy) > 0)
        {
            _IsInitialSetup = false;
            Console.Error.WriteLine($"Found Enemy units!");
            if (!_IsBarracksBuilt)
            {
                var closestBarracksSite = SiteDistances.First(x => _BarracksSites.Any(s => s.SiteId ==x.Key.SiteId)).Key;
                Console.Error.WriteLine($"Building Barracks on Site {closestBarracksSite.SiteId}!");
                return Commands.Build(closestBarracksSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (touchedSite != null)
            {
                var command = GetBuildTouchedSiteCommand(touchedSite);
                if (command != null)
                {
                    return command;
                }
            }
            if (!areThereEnoughTowers)
            {
                Console.Error.WriteLine($"Building Tower!");
                string? buildTowerCommand = GetBuildTowerCommand(queen);
                if (buildTowerCommand != null)
                {
                    return buildTowerCommand;
                }
            }
            var (closestEnemyKnight, closestUnitDistance) = GetClosestEnemyKnight(field, sites, queen);

            if (closestUnitDistance < 400)
            {
                if (_MyTowers > 0)
                {
                    var enemyBarracks = GetEnemyBarracks(sites);
                    if (enemyBarracks != null)
                    {
                        Site? furthestTower = GetFurthestTowerFromFieldItem(sites, enemyBarracks!);
                        if (furthestTower != null)
                        {
                            return Commands.Run(furthestTower, closestEnemyKnight!);
                        }
                    }
                    
                }
            }
        }

        Console.Error.WriteLine($"_IsInitialSetup= {_IsInitialSetup}.");
        if (_IsInitialSetup)
        {
            if (touchedSite != null)
            {
                var command = GetBuildTouchedSiteCommand(touchedSite);
                if (command != null)
                {
                    return command;
                }
            }
            Console.Error.WriteLine($"!_IsBarracksBuilt = {!_IsBarracksBuilt}, _BarracksSites.Any(s => s == closestNonHostileSite) = {_BarracksSites.Any(s => s == closestNonHostileSite)}.");
            if (!_IsBarracksBuilt && _BarracksSites.Any(s => s == closestNonHostileSite))
            {
                return Commands.Build(closestNonHostileSite!.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (closestNonHostileSite != null)
            {
                if (closestNonHostileSite.RemainingGold > 50)
                {
                    return Commands.Build(closestNonHostileSite.SiteId, StructureType.MINE, null);
                }
                else
                {
                    return Commands.Build(closestNonHostileSite.SiteId, StructureType.TOWER, null);
                }
            }
        }
        else
        {
            if (!_IsBarracksBuilt)
            {
                var closestBarracksSite = SiteDistances.First(x => _BarracksSites.Any(s => s.SiteId ==x.Key.SiteId)).Key;
                Console.Error.WriteLine($"Building Barracks on Site {closestBarracksSite.SiteId}!");
                return Commands.Build(closestBarracksSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (touchedSite != null)
            {
                var command = GetBuildTouchedSiteCommand(touchedSite);
                if (command != null)
                {
                    return command;
                }
            }
            if (!areThereEnoughMines && closestNonHostileSite != null )
            {
                
                // TODO if no gold in mine dont go and guild mine there. Or enemies
                if (closestNonHostileSite.RemainingGold > 50)
                {
                    return Commands.Build(closestNonHostileSite.SiteId, StructureType.MINE, null);
                }
                else
                {
                    return Commands.Build(closestNonHostileSite.SiteId, StructureType.TOWER, null);
                }
            }
        }
            return Commands.Wait();
    }

    private Site? GetEnemyBarracks(List<Site> sites)
    {
        return sites.FirstOrDefault(s => s.Owner == Owner.Enemy && s.Structure.Type == StructureType.BARRACKS);
    }

    private bool IsEnemyTraining(List<Site> sites)
    {
        foreach (Site site in sites)
        {
            if (site.Owner == Owner.Enemy && site.Structure.Type == StructureType.BARRACKS && ((Barracks)site.Structure).IsTraining)
            {
                return true;
            }
        }
        return false;
    }

    private Site? GetFurthestTowerFromFieldItem(List<Site> sites, IFieldItem closestEnemyKnight)
    {
        Site? furthest = null;
        int distance = int.MaxValue;

        foreach (Site site in sites)
        {
            if (site.Owner != Owner.Friendly || site.Structure.Type != StructureType.TOWER)
            {
                continue;
            }

            var newDistance = GetDistance(site, closestEnemyKnight);
            if (furthest == null)
            {
                furthest = site;
                distance = newDistance;
            }
            else if (newDistance > distance)
            {
                furthest = site;
                distance = newDistance;
            }
        }
        return furthest;
    }

    private string? GetBuildTowerCommand(Queen queen)
    {
        Site? site = null;
        if (_TowerLocations[0].Structure.Type != StructureType.TOWER)
        {
            if (_TowerLocations[1].Structure.Type == StructureType.TOWER)
            {
                site = _TowerLocations[0];
            }
            else
            {
                var distance0 = GetDistance(_TowerLocations[0], queen);
                var distance1 = GetDistance(_TowerLocations[1], queen);
                site = distance0 < distance1 ? _TowerLocations[0] : _TowerLocations[1];
            }
        }
        else if (_TowerLocations[1].Structure.Type != StructureType.TOWER)
        {
            site = _TowerLocations[1];
        }
        if (site == null)
        {
            return null;
        }
        return Commands.Build(site.SiteId, StructureType.TOWER);
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
            if (!IsFieldItemOnMySide(siteA) || siteA.Equals(_MyBarracks))
            {
                continue;
            }
            foreach (var siteB in sites)
            {
                if (siteA.Equals(siteB) ||
                    !IsFieldItemOnMySide(siteB) || 
                    GetDistance(siteA, siteB) > maxTowerRange ||
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

    private bool IsFieldItemOnMySide(Site siteA)
    {
        return siteA.X > _MySide[0] && siteA.X < _MySide[1];
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

    private (Knight? unit, int closestUnitDistance) GetClosestEnemyKnight(Field field, List<Site> sites, Queen queen)
    {
        Knight? closest = null;
        int distance = int.MaxValue;
        foreach(var unit in field.Units)
        {
            if (unit.Type != UnitType.KNIGHT || unit.Owner != Owner.Enemy)
            {
                continue;
            }
            var unitDistance = GetDistance(queen, unit);
            if (distance > unitDistance)
            {
                distance = unitDistance;
                closest = (Knight)unit;
            }
        }
        return (closest, distance);
    }

    private IOrderedEnumerable<KeyValuePair<Site, int>> GetSitesAndDistance(List<Site> sites, Queen queen)
    {
        var siteDistances = new Dictionary<Site, int>();
        foreach (var site in sites)
        {
            var distance = GetDistance(queen, site);
            siteDistances.Add(site, distance);
        }

        return siteDistances.OrderBy(x => x.Value);
    }

    private string? GetBuildTouchedSiteCommand(Site? touchedSite)
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

    private int GetDistance(IFieldItem fieldItemA, IFieldItem fieldItemB)
    {
        var xDistance = Math.Abs(fieldItemA.X - fieldItemB.X) - (fieldItemB.Radius + fieldItemA.Radius);
        var yDistance = Math.Abs(fieldItemA.Y - fieldItemB.Y) - (fieldItemB.Radius + fieldItemA.Radius);
        return (xDistance + yDistance);
    }
}

// Check for closest site maxGold if low then build archer
// then go to middle build mine max upgrade
// go vertical build tower upgrade.
// go vertical build mine & upgrade
// go vertical build barracks
// go Horizontal buildTower