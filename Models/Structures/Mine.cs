public class Mine : IStructure
{
    public StructureType Type => StructureType.MINE;
    public int IncomeRate { get; set; }
    public void Update(int incomeRate, int notUsed)
    {
        IncomeRate = incomeRate;
    }
}