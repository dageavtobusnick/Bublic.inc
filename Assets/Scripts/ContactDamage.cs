using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField]
    private int _damage;
    [SerializeField]
    private int _teamId;

    public int Damage { get => _damage;}

    public void InitDamage(int damage)
    {
        _damage = damage;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out var hPBar))
            hPBar.TakeDamage(_teamId,_damage);
    }
}
