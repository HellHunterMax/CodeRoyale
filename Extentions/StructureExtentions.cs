public static class StructureExtentions
{
    public static (Site?, int) FindClosest(this StructureType type, List<Site> sites, Owner owner, Queen queen)
    {
        int distance = int.MaxValue;
        Site? closestSite = null;
        foreach (var site in sites)
        {
            if (site.Structure.Type != type || site.Owner == Owner.Enemy)
            {
                continue;
            }
            var unitDistance = queen.GetDistance(site);
            if (distance > unitDistance)
            {
                distance = unitDistance;
                closestSite = site;
            }
        }
        return (closestSite, distance);
    }

    public static Site? GetSite(this List<Site> sites, int id)
    {
        return sites.FirstOrDefault(x=> x.SiteId == id);
    }
}