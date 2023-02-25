using UnityEngine;

public abstract class UIItemBase : MonoBehaviour
{
    private void Start()
    {
        var item = GetComponent<InventoryItemScript>();
        item.Equip += OnEquip;
        item.Deequip += OnDeequip;
    }
    protected abstract void OnEquip(ItemScript item);


    protected abstract void OnDeequip(ItemScript item);
}
