public class GameStatesFactory
{
    public GameState GetGameState(Tactics tactics, Game game)
    {
        if (game.Field.CountUnitsOf(UnitType.KNIGHT, Owner.Enemy) > 0)
        {
            return new EnemyCloseSatet(tactics, game);
        }
        if (GetEnemyBarracks(game.Sites) != null)
        {
            return new EndGameState(tactics, game);
        }
        return new InitialSetupState(tactics, game);
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
}