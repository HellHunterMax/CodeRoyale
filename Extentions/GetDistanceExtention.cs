public static class GetDistanceExtention
{
    public static int GetDistance(this Queen queen, IFieldItem fieldItem)
    {
        var xDistance = Math.Abs(queen.X - fieldItem.X) - fieldItem.Radius;
        var yDistance = Math.Abs(queen.Y - fieldItem.Y) - fieldItem.Radius;
        return (xDistance + yDistance);
    }
}