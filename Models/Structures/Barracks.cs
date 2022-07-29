public class Barracks : IStructure
{
    public UnitType UnitType { get; set; }
    public bool IsTraining { get; set; }
    public StructureType Type => StructureType.BARRACKS;

    public void Update(int trainingNumber, int unitType)
    {
        IsTraining = trainingNumber == 0 ? false : true;
        UnitType = unitType.ToUnitType();
    }
}