using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public InventoryItem InventoryItem;
}

[System.Serializable]
public class InventoryItem
{
    public int Id;
    public GameObject MainObject;
    public Sprite Icon;
    public InventoryItem(GameObject mainObject, int id, Sprite icon)
    {
        MainObject = mainObject;
        Id = id;
        Icon = icon;
    }
}
