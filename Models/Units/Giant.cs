public class Giant : Unit, IBuildableUnit
{
    public override UnitType Type { get; } = UnitType.GIANT;
    public Giant(Owner owner, int x, int y, int hp) : base(owner, x, y, 100)
    {
    }

    public static int GoldCost => 130;

}