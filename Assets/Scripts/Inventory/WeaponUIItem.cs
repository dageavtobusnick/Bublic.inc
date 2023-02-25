using UnityEngine;

public class WeaponUIItem :UIItemBase
{
    protected override void OnEquip(ItemScript item)
    {
        if (item != null && item.TryGetComponent<MeleeWeapon>(out var weapon))
        {
            weapon.Equip();
        }
    }

    protected override void OnDeequip(ItemScript item)
    {
        if (item != null && item.TryGetComponent<MeleeWeapon>(out var weapon))
        {
            weapon.Deequip();
        }
    }
}
