public class MovesV2 : IMoveImplant
{
    private bool _IsInitialSetup = true;
    private int _MyMines = 0;
    private int _MyBarracks = 0;
    private int _MyTowers = 0;
    private Site _BarracksSite;

    public MovesV2(Field field, List<Site> sites, Queen queen)
    {
        var SiteDistances = GetSitesAndDistance(sites, queen);
        var isMySideLeft = queen.X < Field.MaxWidth/2 ? true : false;
        _BarracksSite = GetBestBarrackPlace(SiteDistances, isMySideLeft);
    }

    private Site GetBestBarrackPlace(IOrderedEnumerable<KeyValuePair<Site, int>> siteDistances, bool isMySideLeft)
    {
        var middle = Field.MaxWidth / 2;
        Site? bestSite = null;
        if (isMySideLeft)
        {
            foreach (var site in siteDistances)
            {
                if (site.Key.X < middle)
                {
                    if (bestSite == null)
                    {
                        bestSite = site.Key;
                        continue;
                    }
                    if (bestSite.MaxMineSize > site.Key.MaxMineSize && bestSite.RemainingGold > site.Key.RemainingGold)
                    {
                        bestSite = site.Key;
                    }
                }
            }
        }
        else
        {
            foreach (var site in siteDistances)
            {
                if (site.Key.X > middle)
                {
                    if (bestSite == null)
                    {
                        bestSite = site.Key;
                        continue;
                    }
                    if (bestSite.MaxMineSize > site.Key.MaxMineSize && bestSite.RemainingGold > site.Key.RemainingGold)
                    {
                        bestSite = site.Key;
                    }
                }
            }
        }
        Console.Error.WriteLine($"Best Barracks Site is: {bestSite!.SiteId}");
        return bestSite;
    }

    public string GetMoveCommand(Field field, List<Site> sites, Queen queen)
    {
        _MyMines = CountMines(sites);
        _MyBarracks = CountBarracks(sites);
        _MyTowers = CountTowers(sites);

        var SiteDistances = GetSitesAndDistance(sites, queen);

        var (closestNonHostileSite, distance) = GetClosestNonHostileSite(field, sites, queen);
        var areThereEnoughMines = _MyMines > 3;
        var areThereEnoughBarracks = _MyBarracks > 0;
        var areThereEnoughTowers = _MyTowers > 1;
        var touchedSite = sites.FirstOrDefault(x=> x.SiteId == queen.TouchedSite);

            // Check for closest site maxGold if low then build archer
            // then go to middle build mine max upgrade
            // go vertical build tower upgrade.
            // go vertical build mine & upgrade
            // go vertical build barracks
            // go Horizontal buildTower
            // TODO if no gold in mine dont go and guild mine there.
        if  (field.CountUnitsOf(UnitType.KNIGHT, Owner.Enemy) > 0)
        {
            Console.Error.WriteLine($"Found Enemy units!"); //Build Towers and Run!
            _IsInitialSetup = false;
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
            if (closestNonHostileSite == _BarracksSite)
            {
                return Commands.Build(closestNonHostileSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (closestNonHostileSite != null)
            {
                if (closestNonHostileSite.RemainingGold > 20)
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
            if (_BarracksSite.Structure.Type != StructureType.BARRACKS)
            {
                return Commands.Build(_BarracksSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
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
                return Commands.Build(closestNonHostileSite.SiteId, StructureType.TOWER, null);
            }
            if (!areThereEnoughMines)
            {
                if (closestNonHostileSite.RemainingGold > 20)
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