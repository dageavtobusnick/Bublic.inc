using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponUIItem))]
[RequireComponent(typeof(ModuleUIItem))]
public class InventoryItemScript : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    [SerializeField]
    private Canvas _canvas;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private ItemScript _item;
    private Image _image;
    private Vector2 _oldPosition;
    private bool _needEquipement;

    public bool IsEmpty { get => _item == null;}
    public ItemScript Item { get => _item;}

    public event Action<ItemScript> Equip;
    public event Action<ItemScript> Deequip;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _image = GetComponent<Image>();
    }

    public void NeedEquipement()
    {
        _needEquipement = true;
    }

    public ItemScript RemoveItem()
    {
        var item=_item;
        _item = null;
        _image.sprite=null;
        _image.enabled=false;
        if (_needEquipement)
            Deequip?.Invoke(item);
        return item;
    }

    public ItemScript Replace(ItemScript item)
    {
        var oldItem = RemoveItem();
        AddItem(item);
        return oldItem;
    }

    public bool IsWeapon()
    {
        return (_item != null) && _item.TryGetComponent<MeleeWeapon>(out var _);
    }
    public bool IsModule()
    {
        return (_item != null) && _item.TryGetComponent<ModuleScript>(out var _);
    }


    public void AddItem(ItemScript item)
    {
        _item=item;
        _image.sprite=item?.InventoryItem?.Icon;
        _image.enabled=item!=null;
        if(_needEquipement)
            Equip?.Invoke(item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = .6f;
        _oldPosition = _rectTransform.anchoredPosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta/_canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        _rectTransform.anchoredPosition = _oldPosition;
    }
}
