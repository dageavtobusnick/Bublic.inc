using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(ContactDamage))]
[RequireComponent(typeof(ABPath))]
[RequireComponent(typeof(HP))]
public class JagerStats : MonoBehaviour
{
    [SerializeField]
    private int _hP;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _atackTime;
    [SerializeField]
    private float _atackKD;
    [SerializeField]
    private int _damage;

    public float RotateSpeed { get => _rotateSpeed;}
    public float AtackTime { get => _atackTime;}
    public float AtackKD { get => _atackKD;}

    void Start()
    {
        GetComponent<ContactDamage>().InitDamage(_damage);
        GetComponentInChildren<ContactDamage>().InitDamage(_damage);
        GetComponent<AIPath>().maxSpeed = _speed;
        GetComponent<IDamagable>().InitHealth(_hP);
        GetComponentInChildren<CircleCollider2D>().radius = 1.3f + _range;
        transform.GetChild(0).position += new Vector3(0, _range, 0);
    }
}
