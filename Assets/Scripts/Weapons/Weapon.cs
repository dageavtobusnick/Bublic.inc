public interface IWeapon 
{
    public int Damage { get; set; }
    public int Durability { get; set; }
    public float AttackCooldown { get; set; }
    public float StartTimeAttack { get; set; }
    void DoDamage();

}
