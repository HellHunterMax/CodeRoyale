public enum Owner
{
    Neutral,
    Friendly,
    Enemy
}

public static class OwnerMapper 
{
    public static Owner ToOwner(this int number) => number switch
    {
        -1 => Owner.Neutral,
        0 => Owner.Friendly,
        1 => Owner.Enemy,
        _ => throw new ArgumentOutOfRangeException($"Unknown Owner for number: {number}"),
    };

    public static int ToInt(this Owner owner) => owner switch
    {
        Owner.Neutral => -1,
        Owner.Friendly => 0,
        Owner.Enemy => 1,
        _ => throw new ArgumentOutOfRangeException($"Unknown number for Owner: {owner}"),
    };
}