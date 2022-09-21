class Player
{
    static void Main(string[] args)
    {
        var game = new Game();
        var tactics = new Tactics(game);
        
        ITrainImplant trainImplant = new TrainV1();
        var ai = new Ai(trainImplant, tactics, new GameStatesFactory());

        while (true)
        {
            if (!game.IsTurn1)
            {
                game.ReadGameLoop();
            }
            game.IsTurn1 = false;
            Console.WriteLine(ai.GetMoveCommand(game));
            Console.WriteLine(ai.GetTrainCommand(game.Field, game.Sites, game.Queen));
            game.ResetField();
        }
    }
}