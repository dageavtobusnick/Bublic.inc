using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;
    [HideInInspector]
    public WeaponStats Weapon;
    [HideInInspector]
    public Vector3 Direction;
    [HideInInspector]
    public int TeamId;

    [SerializeField]
    private float _speed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Direction * _speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision==null||collision.gameObject==null||collision.gameObject==Owner) return;
        if ( collision.CompareTag("Room") || collision.CompareTag("Exit")||collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        if (collision.TryGetComponent<IDamagable>(out var hp))
        {
            hp.TakeDamage(TeamId, Weapon.Damage);
        }
    }
}
