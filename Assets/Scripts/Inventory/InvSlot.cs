using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour,IDropHandler
{
    [SerializeField]
    private InventoryItemScript _item;

    protected void MakeEquipable()
    {
        _item.NeedEquipement();
    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var item = eventData.pointerDrag.GetComponent<InventoryItemScript>();
            if (item != _item)
            {
                var t1 = _item.RemoveItem();
                var t2 = item.Replace(t1);
                _item.AddItem(t2);
            }
        }
    }
}
