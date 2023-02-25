using System;

public interface IDamagable
{
    int CurrentHp { get; }
    int MaxHp { get; }

    void InitHealth(int hp);
    void TakeDamage(int teamId, int damage);
    public event Action Damaged;
    public event Action Dead;
}
