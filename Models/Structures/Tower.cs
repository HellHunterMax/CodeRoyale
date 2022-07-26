public class Tower : Structure
{
    public override StructureType Type => StructureType.TOWER;
    public int HP { get; set;}
    public int AttackRadius { get; set; }
    public override void Update(int hp, int attackRadius)
    {
        HP = hp;
        AttackRadius = attackRadius;
    }
}