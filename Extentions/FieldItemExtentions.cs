public static class FieldItemExtentions
{
    public static int GetDistanceTo(this IFieldItem fieldItemA, IFieldItem fieldItemB)
    {
        var xDistance = Math.Abs(fieldItemA.X - fieldItemB.X) - (fieldItemB.Radius + fieldItemA.Radius);
        var yDistance = Math.Abs(fieldItemA.Y - fieldItemB.Y) - (fieldItemB.Radius + fieldItemA.Radius);
        return (xDistance + yDistance);
    }
}