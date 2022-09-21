public class GameRepository
{
    private readonly Game _game;

    public GameRepository(Game game)
    {
        _game = game;
    }

    public Site? GetFirstSite(Func<Site, bool> func)
    {
        return _game.Sites.FirstOrDefault(func);
    }

    public Site? GetSite(int id)
    {
        return _game.Sites.FirstOrDefault(x=> x.SiteId == id);
    }

}