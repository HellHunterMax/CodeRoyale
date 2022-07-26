public class Mine : Structure
{
    public override StructureType Type => StructureType.MINE;
    public int IncomeRate { get; set; }
    public override void Update(int incomeRate, int notUsed)
    {
        IncomeRate = incomeRate;
    }
}