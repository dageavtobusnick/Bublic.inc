using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField]
    private List<InventoryItemScript> _modules;
    [SerializeField]
    private List<InventoryItemScript> _fastSlots;
    [SerializeField]
    private InventoryItemScript _weapon;


    private List<InventoryItemScript> _inventoryItems;


    void Start()
    {
        _inventoryItems = FindObjectsOfType<InventoryItemScript>().ToList();
        _inventoryItems.Remove(_weapon);
        foreach (var e in _modules)
        {
            _inventoryItems.Remove(e);
            e.Equip += OnModuleEquiped;
        }
    }

    public void OnModuleEquiped(ItemScript module)
    {
        if(TryCraftTripable(out var tripable))
        {
            foreach(var e in _modules)
            {
                Destroy(e.RemoveItem().gameObject);
            }
            var newObject=Instantiate(tripable,GameController.Player.transform).GetComponent<ItemScript>();
            newObject.GetComponent<IPickable>().PickUp();
        }

    }

    public bool TryCraftTripable(out ItemScript tripable)
    {
        var firstItem= _modules[0].Item;
        tripable = null;
        foreach(var e in _modules)
        {
            if (firstItem?.InventoryItem?.Id != e.Item?.InventoryItem?.Id)
                return false;
        }
        tripable = firstItem.GetComponent<ModuleScript>().TripletModule;
        return tripable!=null;
    }

    public void Use(int number)
    {
        var from = _fastSlots[number];
        EquipModuleInFreeSlot(from);
        EquipWeapon(from);
        Heal(from);
    }

    private void Heal(InventoryItemScript from)
    {
        if(from.Item!=null&&from.Item.TryGetComponent<HealScript>(out var heal))
        {
            heal.Heal();
            from.RemoveItem();
            Destroy(heal);
        }
    }

    public void EquipWeapon(InventoryItemScript from)
    {
        if(from.IsWeapon())
            from.AddItem(_weapon.Replace(from.RemoveItem()));
    }

    private void EquipModule(InventoryItemScript from,InventoryItemScript to)
    {
        from.AddItem(to.Replace(from.RemoveItem()));
    }
    public void EquipModuleInFreeSlot(InventoryItemScript from)
    {
        if(from.IsModule())
            foreach(var e in _modules)
            {
                if(e.IsEmpty)
                {
                    EquipModule(from, e);
                }
            }
    }
    public void AddItem(ItemScript itemScript)
    {
        foreach(var e in _inventoryItems)
        {
            if (e.IsEmpty)
            {
                e.AddItem(itemScript);
                break;
            }
        }
    }

    public void CollectItem(GameObject itemObject)
    {
        if (itemObject != null)
        {
            if (itemObject.TryGetComponent<ItemScript>(out var item))
            {
                AddItem(item);
                item.transform.SetParent(GameController.Player.transform);
                item.transform.position.Set(0, 0, 4);
                item.gameObject.SetActive(false);
            }
        }
    }
}

