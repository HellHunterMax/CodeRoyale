public class InitialSetupState : GameState
{
    public InitialSetupState(Tactics tactics, Game game) : base(tactics, game)
    {
    }

    public override string GetMoveCommand()
    {
        var touchedSite = _Game.Sites.GetSite(_Game.Queen.TouchedSite);
        if (touchedSite != null)
            {
                var command = Commands.BuildTouchedSiteCommand(touchedSite);
                if (command != null)
                {
                    return command;
                }
            }
            Console.Error.WriteLine($"!_IsBarracksBuilt = {!_Tactics._IsBarracksBuilt}, _BarracksSites.Any(s => s == closestNonHostileSite) = {_Tactics._BarracksSites.Any(s => s == _ClosestNonHostileSite)}.");
            if (!_Tactics._IsBarracksBuilt && _Tactics._BarracksSites.Any(s => s == _ClosestNonHostileSite))
            {
                return Commands.Build(_ClosestNonHostileSite!.SiteId, StructureType.BARRACKS, UnitType.KNIGHT);
            }
            if (_ClosestNonHostileSite != null)
            {
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