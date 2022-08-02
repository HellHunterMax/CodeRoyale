class Player
{
    static void Main(string[] args)
    {
        var game = new Game();
        
        IMoveImplant moveImplant = new MovesV2(game.Field, game.Sites, game.Queen);
        ITrainImplant trainImplant = new TrainV1();
        var ai = new Ai(moveImplant, trainImplant);

        game.Run(ai);
    }
}