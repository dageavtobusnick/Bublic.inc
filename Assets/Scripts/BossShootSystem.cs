using UnityEngine;

public class BossShootSystem : ShootSystem
{
    [SerializeField]
    private GameObject _boss;

    protected override void Shoot()
    {
        Firepoint = gameObject.transform;
        var bullet = Instantiate(Ammo, gameObject.transform.position, Firepoint.rotation).GetComponent<Bullet>();
        bullet.Owner = _boss;
        bullet.Weapon = WeaponStats;
        bullet.Direction=(gameObject.transform.position-_boss.transform.position).normalized;
        bullet.TeamId = GameController.UndeadId;
    }

}
