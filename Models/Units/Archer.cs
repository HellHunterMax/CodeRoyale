public class Archer : Unit, IBuildableUnit
{
    public override UnitType Type { get; } = UnitType.ARCHER;
    public static int GoldCost => 100;
    public Archer(Owner owner, int x, int y, int hp) : base(owner, x, y, 100)
    {
    }
}