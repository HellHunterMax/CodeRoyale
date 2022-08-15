class Player
{
    static void Main(string[] args)
    {
        var game = new Game();
        
        IMoveImplant moveImplant = new MovesV2(game.Field, game.Sites, game.Queen);
        ITrainImplant trainImplant = new TrainV1();
        var ai = new Ai(moveImplant, trainImplant);

        while (true)
        {
            if (!game.IsTurn1)
            {
                game.ReadGameLoop();
            }
            game.IsTurn1 = false;
            Console.WriteLine(ai.GetMoveCommand(game.Field, game.Sites, game.Queen));
            Console.WriteLine(ai.GetTrainCommand(game.Field, game.Sites, game.Queen));
            game.ResetField();
        }
    }
}