public class EnemyCloseSatet : GameState
{
    public EnemyCloseSatet(Tactics tactics, Game game) : base(tactics, game)
    {
    }

    public override string GetMoveCommand()
    {
        if (!_Tactics._IsBarracksBuilt)
        {
            var closestBarracksSite = _SiteDistances.First(x => _Tactics._BarracksSites.Any(s => s.SiteId ==x.Key.SiteId)).Key;
            Console.Error.WriteLine($"Building Barracks on Site {closestBarracksSite.SiteId}!");
            return Commands.Build(closestBarracksSite.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
        }
        var touchedSite = _Game.Sites.GetSite(_Game.Queen.TouchedSite);
        if (touchedSite != null)
        {
            var command = Commands.BuildTouchedSiteCommand(touchedSite);
            if (command != null)
            {
                return command;
            }
        }
        if (!_Tactics._AreThereEnoughTowers)
        {
            Console.Error.WriteLine($"Building Tower!");
            string? buildTowerCommand = Commands.BuildTowerCommand(_Game.Queen, _Tactics._TowerLocations);
            if (buildTowerCommand != null)
            {
                return buildTowerCommand;
            }
        }
        var (closestEnemyKnight, closestUnitDistance) = GetClosestEnemyKnight(_Game.Field, _Game.Sites, _Game.Queen);

        if (closestUnitDistance < 400)
        {
            if (_Tactics._MyTowers > 0)
            {
                var enemyBarracks = GetEnemyBarracks(_Game.Sites);
                if (enemyBarracks != null)
                {
                    Site? furthestTower = GetFurthestTowerFromFieldItem(_Game.Sites, enemyBarracks!);
                    if (furthestTower != null)
                    {
                        return Commands.Run(furthestTower, closestEnemyKnight!);
                    }
                }
                
            }
        }
        return Commands.Wait();
    }
    private Site? GetEnemyBarracks(List<Site> sites)
    {
        return sites.FirstOrDefault(s => s.Owner == Owner.Enemy && s.Structure.Type == StructureType.BARRACKS);
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
            var unitDistance = queen.GetDistance(unit);
            if (distance > unitDistance)
            {
                distance = unitDistance;
                closest = (Knight)unit;
            }
        }
        return (closest, distance);
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

            var newDistance = site.GetDistanceTo(closestEnemyKnight);
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
}