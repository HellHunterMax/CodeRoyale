public class EndGameState : GameState
{
    public EndGameState(Tactics tactics, Game game) : base(tactics, game)
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
            if (!_Tactics._AreThereEnoughMines && _ClosestNonHostileSite != null )
            {
                
                // TODO if no gold in mine dont go and guild mine there. Or enemies
                if (_ClosestNonHostileSite.RemainingGold > 50)
                {
                    return Commands.Build(_ClosestNonHostileSite.SiteId, StructureType.MINE, null);
                }
                else
                {
                    return Commands.Build(_ClosestNonHostileSite.SiteId, StructureType.TOWER, null);
                }
            }
        return Commands.Wait();
    }
}
