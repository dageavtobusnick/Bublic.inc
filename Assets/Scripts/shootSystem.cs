using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    [SerializeField]
    private Transform _firepoint;
    [SerializeField]
    private GameObject _ammo;


    private float _reloadTime; 
    private float _shootKD = 0f;
    private bool _isShooting;
    private WeaponStats _weaponStats;
    private int _shootCount = 0;
    private int _teamId;

    public Transform Firepoint { get => _firepoint; protected set => _firepoint = value; }
    public int ShootCount { get => _shootCount;}
    public bool IsShooting { get => _isShooting; set => _isShooting = value; }
    public WeaponStats WeaponStats { get => _weaponStats; set => _weaponStats = value; }
    public GameObject Ammo { get => _ammo;}

    private void Start()
    { 
        if (TryGetComponent<WeaponStats>(out var stats))
        {
            _weaponStats = stats;
            _reloadTime = _weaponStats.ReloadTime;
        }
        if(TryGetComponent<IEntity>(out var entity))
        {
            _teamId = entity.TeamId;
        }
    }

    void Update()
    {
        if (IsShooting)
            if (_shootKD <= 0)
            {
                Shoot();
                _shootCount++;
                _shootKD = _reloadTime;
            }
            else _shootKD -= Time.deltaTime;
    }

    public void StartShooting()
    {
        _isShooting = true;
        _reloadTime = _weaponStats.ReloadTime;
    }
    public void ClearShoots()
    {
        _shootCount = 0;
    }

    public void LoadWeaponData(WeaponStats stats)
    {
        _weaponStats= stats;
        _reloadTime= _weaponStats.ReloadTime;
    }

    public void SetTargetOnPlayer()
    {
        _firepoint = GameController.Player.transform;
    }

    public void InstantReload()
    {
        _shootKD = 0;
    }

    protected virtual void Shoot()
    {
        var bullet = Instantiate(_ammo, gameObject.transform.position, _firepoint.rotation).GetComponent<Bullet>();
        bullet.Owner = gameObject;
        bullet.Weapon = _weaponStats;
        bullet.Direction = (_firepoint.position - gameObject.transform.position).normalized;
        bullet.TeamId = _teamId;
    }
}
