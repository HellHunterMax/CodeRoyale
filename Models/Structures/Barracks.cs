public class Barracks : Structure
{
    public UnitType UnitType { get; set; }
    public bool IsTraining { get; set; }
    public override StructureType Type => StructureType.BARRACKS;

    public override void Update(int trainingNumber, int unitType)
    {
        IsTraining = trainingNumber == 0 ? false : true;
        UnitType = unitType.ToUnitType();
    }
}