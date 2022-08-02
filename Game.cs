public class Game
{
    public Field Field { get; } = new Field();
    public List<Site> Sites { get; } = new List<Site>();
    public Queen Queen {get;} = new Queen(Owner.Friendly, 0, 0, 100);

    private int _numSites;

    public Game()
    {
        SetupGame(out _numSites);
        ReadGameLoop();
    }

    public void Run(Ai ai)
    {
        bool isTurn1 = true;
        // game loop
        while (true)
        {
            if (!isTurn1)
            {
                ReadGameLoop();
            }
            isTurn1 = false;
            Console.WriteLine(ai.GetMoveCommand(Field, Sites, Queen));
            Console.WriteLine(ai.GetTrainCommand(Field, Sites, Queen));
            ResetField();
        }
    }

    private void ResetField()
    {
        Field.Units.Clear();
    }

    private void ReadGameLoop()
    {
        var inputs = Console.ReadLine()!.Split(' ');
        Queen.Gold = int.Parse(inputs[0]);
        Queen.TouchedSite = int.Parse(inputs[1]); // -1 if none

        for (int i = 0; i < _numSites; i++)
        {
            ReadSite();
        }
        int numUnits = int.Parse(Console.ReadLine()!);
        for (int i = 0; i < numUnits; i++)
        {
            ReadUnit();
        }

    }

    private void ReadUnit()
    {
        string[] inputs = Console.ReadLine()!.Split(' ');
        int x = int.Parse(inputs[0]);
        int y = int.Parse(inputs[1]);
        int owner = int.Parse(inputs[2]); // 0 = Friendly, 1 = Enemy
        int unitType = int.Parse(inputs[3]); // -1 = QUEEN, 0 = KNIGHT, 1 = ARCHER, 2 = Giant
        int health = int.Parse(inputs[4]);

        AddUnitsToField(owner, unitType, health, x, y);
    }

    private void ReadSite()
    {
        string[] inputs = Console.ReadLine()!.Split(' ');
        int siteId = int.Parse(inputs[0]);
        int gold = int.Parse(inputs[1]); // used in future leagues
        int maxMineSize = int.Parse(inputs[2]); // used in future leagues
        int structureType = int.Parse(inputs[3]); // -1 = No structure, 1 = TOWER, 2 = Barracks
        int owner = int.Parse(inputs[4]); // -1 = No structure, 0 = Friendly, 1 = Enemy
        int param1 = int.Parse(inputs[5]);//When barracks, the number of turns before a new set of creeps can be trained (if 0, then training may be started this turn)
        int param2 = int.Parse(inputs[6]);//When barracks: the creep type: 0 for KNIGHT, 1 for ARCHER

        Site site = GetSite(siteId);
        site.UpdateSite(gold, maxMineSize, owner.ToOwner(), structureType, param1, param2);
    }

    private void AddUnitsToField(int owner, int unitType, int health, int x, int y)
    {
        if(unitType == UnitType.KNIGHT.ToInt())
        {
            Field.Units.Add(new Knight(owner.ToOwner(), x, y, health));
        }
        else if(unitType == UnitType.ARCHER.ToInt())
        {
            Field.Units.Add(new Archer(owner.ToOwner(), x, y, health));
        }
        else if (unitType == UnitType.GIANT.ToInt())
        {
            Field.Units.Add(new Giant(owner.ToOwner(), x, y, health));
        }
        else if(owner == Owner.Friendly.ToInt() && unitType == UnitType.QUEEN.ToInt())
        {
            Queen.UpdateQueen(x, y, health);
            Field.Units.Add(Queen);
        }
        else if (owner == Owner.Enemy.ToInt() && unitType == UnitType.QUEEN.ToInt())
        {
            Field.Units.Add(new Queen(owner.ToOwner(), x, y, health));
        }
    }

    private void SetupGame(out int numSites)
    {
        numSites = int.Parse(Console.ReadLine()!);
        for (int i = 0; i < numSites; i++)
        {
            var inputs = Console.ReadLine()!.Split(' ');
            int siteId = int.Parse(inputs[0]);
            int x = int.Parse(inputs[1]);
            int y = int.Parse(inputs[2]);
            int radius = int.Parse(inputs[3]);

            var site = new Site(siteId, x, y, radius);
            Sites.Add(site);

            Field.AddFieldItem(site, x, y);
        }
    }

    private Site GetSite(int siteId) => Sites.First(s => s.SiteId == siteId);
}