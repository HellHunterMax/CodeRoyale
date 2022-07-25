public class Field
{
    public const int MaxHeight = 1000; // Y
    public const int MaxWidth = 1920; // X

    public List<Archer> FriendlyArchers { get; set; } = new List<Archer>();
    public List<Knight> FriendlyKnights { get; set; } = new List<Knight>();

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