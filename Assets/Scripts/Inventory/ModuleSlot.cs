using UnityEngine.EventSystems;

public class ModuleSlot : InvSlot
{
    private void Start()
    {
        MakeEquipable();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventoryItemScript>().IsModule())
            base.OnDrop(eventData);
    }
}
