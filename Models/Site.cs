public class Site : IFieldItem
{
    public int SiteId { get; set; }
    public Owner Owner { get; set; }
    public int RemainingGold { get; set; }
    public int MaxMineSize { get; set; }
    public Structure Structure { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Radius { get; set; }

    public Site(int siteId, int x, int y, int radius)
    {
        SiteId = siteId;
        X = x;
        Y = y;
        Radius = radius;
        Owner = Owner.Neutral;
        Structure = new Empty();
    }

    public void UpdateSite(int gold, int maxMineSize, Owner owner, int structureTypeInt, int param1, int param2)
    {
        RemainingGold = gold;
        MaxMineSize = maxMineSize;
        Owner = owner;
        var structureType = structureTypeInt.ToStructureType();

        if (Structure.Type != structureType)
        {
            BuildStructure(structureType);
        }
        
            Structure.Update(param1, param2);
    }

    private void BuildStructure(StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.EMPTY:
                Structure = new Empty();
                break;
            case StructureType.MINE:
                Structure = new Mine();
                break;
            case StructureType.TOWER:
                Structure = new Tower();
                break;
            case StructureType.BARRACKS:
                Structure = new Barracks();
                break;
            default:
                throw new ArgumentException($"Dont know which building to build for structureType: {structureType}");
        }
    }
}