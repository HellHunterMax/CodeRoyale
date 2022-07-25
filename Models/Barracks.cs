public class Barracks
{
    public UnitType UnitType { get; set; }
    public bool IsTraining { get; set; }

    public Barracks(UnitType unitType)
    {
        UnitType = unitType;
    }

    public void SetTraining(int TrainingNumber)
    {
        IsTraining = TrainingNumber == 0 ? false : true;
    }
}