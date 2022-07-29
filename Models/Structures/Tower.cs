public class Tower : IStructure
{
    public StructureType Type => StructureType.TOWER;
    public int HP { get; set;}
    public int AttackRadius { get; set; }
    public void Update(int hp, int attackRadius)
    {
        HP = hp;
        AttackRadius = attackRadius;
    }
}