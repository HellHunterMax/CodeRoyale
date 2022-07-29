public abstract class Unit : IFieldItem
{
    public abstract UnitType Type { get; }
    public Owner Owner { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public int HP { get; set; }

    public Unit(Owner owner, int x, int y, int hp)
    {
        Owner = owner;
        X = x;
        Y = y;
        HP = hp;
    }
}