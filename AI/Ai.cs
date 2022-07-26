public class Ai
{
    private readonly IMoveImplant _moveImplant;
    private readonly ITrainImplant _trainImplant;

    public Ai(IMoveImplant moveImplant, ITrainImplant trainImplant)
    {
        this._moveImplant = moveImplant;
        this._trainImplant = trainImplant;
    }

    public string GetMoveCommand(Field field, List<Site> sites, Queen queen) => _moveImplant.GetMoveCommand(field, sites, queen);
    public string GetTrainCommand(Field field, List<Site> sites, Queen queen) => _trainImplant.GetTrainCommand(field, sites, queen);
}