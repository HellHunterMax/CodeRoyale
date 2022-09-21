public static class QueenExtentions
{
    public static int GetDistance(this Queen queen, IFieldItem fieldItem)
    {
        var xDistance = Math.Abs(queen.X - fieldItem.X) - fieldItem.Radius;
        var yDistance = Math.Abs(queen.Y - fieldItem.Y) - fieldItem.Radius;
        return (xDistance + yDistance);
    }
    public static IOrderedEnumerable<KeyValuePair<Site, int>> GetSitesAndDistance(this Queen queen, List<Site> sites)
    {
        var siteDistances = new Dictionary<Site, int>();
        foreach (var site in sites)
        {
            var distance = queen.GetDistance(site);
            siteDistances.Add(site, distance);
        }

        return siteDistances.OrderBy(x => x.Value);
    }
}