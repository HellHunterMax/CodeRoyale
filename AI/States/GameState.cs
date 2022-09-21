public abstract class GameState
{
    internal readonly Tactics _Tactics;
    internal readonly Game _Game;
    internal readonly Site? _ClosestNonHostileSite;
    internal readonly IOrderedEnumerable<KeyValuePair<Site, int>> _SiteDistances;

    public GameState(Tactics tactics, Game game)
    {
        _Tactics = tactics;
        _Game = game;
        _SiteDistances = game.Queen.GetSitesAndDistance(game.Sites);
        var (closestNonHostileSite, closestSiteDistance) = GetClosestNonHostileSite(game.Field, game.Sites, game.Queen);
        _ClosestNonHostileSite = closestNonHostileSite;
        _Tactics._IsBarracksBuilt = _Tactics._BarracksSites.Any(s => s.Structure.Type == StructureType.BARRACKS);
        
    }
    public abstract string GetMoveCommand();
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
            var distance = queen.GetDistance(site);
            closestSite = distance < closestDistance ? site : closestSite;
            closestDistance = distance < closestDistance ? distance : closestDistance;
        }
        return (closestSite, closestDistance);
    }
}