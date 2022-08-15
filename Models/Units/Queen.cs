public class Queen : Unit
{
    public override UnitType Type { get; } = UnitType.QUEEN;
    /// <summary>
    /// MaxMovement = 60
    /// </summary>
    public static int MaxMovement = 60;
    public int Gold { get; set; }
    public int TouchedSite { get; set; }

    public Queen(Owner owner, int x, int y, int hp): base(owner, x, y, hp)
    {
        Radius = 30;
    }

    public void UpdateQueen(int x, int y, int hp)
    {
        X = x;
        Y = y;
        HP = hp;
    }
}