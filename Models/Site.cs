public class Site : IFieldItem
{
    public int SiteId { get; set; }
    public Owner Owner { get; set; }

    public StructureType StructureType { get; set; }
    public Barracks? Barracks { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Radius { get; set; }

    public Site(int siteId, int x, int y, int radius)
    {
        SiteId = siteId;
        X = x;
        Y = y;
        Radius = radius;
        Owner = Owner.Neutral; //No owner
    }
    public void UpdateSite(int structureType, Owner owner, int turnsUntillAvailable, int barrackType)
    {
        StructureType = structureType.MapToStructureType();
        Owner = owner;
        if(structureType != 2)
        {
            return;
        }
        var unitType = barrackType.MapToUnitType();
        if (Barracks is null)
        {
            Barracks = new Barracks(unitType);
        }
        else
        {
            if (Barracks.UnitType != unitType)
            {
                Barracks.UnitType = unitType;
            }
        }
        Barracks.SetTraining(turnsUntillAvailable);
    }
}