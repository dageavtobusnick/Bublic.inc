using System.Linq;
using UnityEngine;

public class BossMainShootSystem : MonoBehaviour
{
    private int _shootCount;
    private bool _isShooting=false;
    private BossShootSystem[] _shootSystems;

    public int ShootCount { get => _shootCount;}

    void Awake()
    {
        _shootSystems = GetComponentsInChildren<BossShootSystem>();
    }
    public bool IsShooting() =>_isShooting;
    public void StartShooting()
    {
        _isShooting = true;
        foreach(var shootSystem in _shootSystems)
        {
            shootSystem.StartShooting();
        }
    }

    void FixedUpdate()
    {
        if (_shootSystems.Length == 0)
        {
            _shootSystems = GetComponentsInChildren<BossShootSystem>();
        }
        if (_isShooting)
        {
            if (_shootSystems.Where(x => x.ShootCount <= _shootCount).Count() <= 0)
            {
                _isShooting = false;
                foreach (var e in _shootSystems)
                {
                    e.ClearShoots();
                }
            }
        }
    }
}
