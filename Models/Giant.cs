public class Giant : Unit, IBuildableUnit
{
    public Giant(Owner owner, int x, int y, int hp) : base(owner, x, y, 100)
    {
    }

    public int GoldCost => 130;

}