public class Queen : Unit, IFieldItem
{
    public const int MaxMovement = 60;
    public static int Radius = 30;
    public int Gold { get; set; }
    public int TouchedSite { get; set; }

    public Queen(Owner owner, int x, int y, int hp): base(owner, x, y, hp)
    {
    }

    public void UpdateQueen(int x, int y, int hp)
    {
        X = x;
        Y = y;
        HP = hp;
    }
}