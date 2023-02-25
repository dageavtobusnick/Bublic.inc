using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _reloadTime;

    public int Damage { get => _damage; }
    public float ReloadTime { get => _reloadTime; }
}
