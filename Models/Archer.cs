public class Archer : Unit, IBuildableUnit
{
    public int GoldCost => 100;
    public Archer(Owner owner, int x, int y, int hp) : base(owner, x, y, 100)
    {
    }
}