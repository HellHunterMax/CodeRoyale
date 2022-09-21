public class Ai
{
    private readonly ITrainImplant _trainImplant;
    private readonly Tactics _tactics;
    private readonly GameStatesFactory _gameStatesFactory;
    private GameState? _gamestate {get; set;}

    public Ai(ITrainImplant trainImplant, Tactics tactics, GameStatesFactory gameStatesFactory)
    {
        _trainImplant = trainImplant;
        _tactics = tactics;
        _gameStatesFactory = gameStatesFactory;
    }

    public string GetMoveCommand(Game game)
    {
        _gamestate = _gameStatesFactory.GetGameState(_tactics, game);
        return _gamestate.GetMoveCommand();
    }
    public string GetTrainCommand(Field field, List<Site> sites, Queen queen) => _trainImplant.GetTrainCommand(field, sites, queen);
}

// Check for closest site maxGold if low then build archer
// then go to middle build mine max upgrade
// go vertical build tower upgrade.
// go vertical build mine & upgrade
// go vertical build barracks
// go Horizontal buildTower

//TODO dont go into enemy Towers if there are no knights in range.
//TODO build more Towers when one of Towerlocations is taken or there  is nearby empty site.
//TODO fix bug when on Enemy Building
//TODO Run away sooner
//TODO Upgrade towers to fullest, start with building the one closest to the frontline