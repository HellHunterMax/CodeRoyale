public class Field
{
    public const int MaxHeight = 1000; // Y
    public const int MaxWidth = 1920; // X

    public List<Unit> Units { get; set; } = new List<Unit>();

    public IFieldItem[][] PlayingField { get; set; }

    public Field()
    {
        PlayingField = CreatePlayingField();
    }

    private IFieldItem[][] CreatePlayingField()
    {
        var playingField = new IFieldItem[MaxWidth][];

        for (int i = 0; i < MaxWidth; i++)
        {
            playingField[i] = new IFieldItem[MaxHeight];
        }
        return playingField;
    }

    public void AddFieldItem(IFieldItem fieldItem, int x, int y)
    {
        PlayingField[x][y] = fieldItem;
    }
}
public static class FieldExtentions
{
    /// <summary>
    /// counts all <see cref="Unit">units</ see> for given <see cref="UnitType">UnitType</ see>
    /// </summary>
    /// <param name="field"></param>
    /// <param name="unitType"></param>
    /// <param name="owner">When Neutral all units will be counted</param>
    /// <returns></returns>
    public static int CountUnitsOf(this Field field, UnitType unitType, Owner owner)
    {
        IEnumerable<Unit> units;
        if (owner != Owner.Neutral)
        {
            units = field.Units.Where(u => u.Owner == owner);
        }
        else
        {
            units = field.Units;
        }
        
        return units.Where(u => u.Type == unitType).Count();
    }
}