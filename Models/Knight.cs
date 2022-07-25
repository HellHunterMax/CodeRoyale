public class Knight : Unit, IBuildableUnit
{
    public int GoldCost => 80;
    public Knight(Owner owner, int x, int y, int hp) : base(owner, x, y, 100)
    {
    }
}