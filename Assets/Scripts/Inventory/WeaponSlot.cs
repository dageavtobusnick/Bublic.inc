using UnityEngine.EventSystems;

public class WeaponSlot : InvSlot
{
    private void Start()
    {
        MakeEquipable();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.GetComponent<InventoryItemScript>().IsWeapon())
            base.OnDrop(eventData);
    }
}
