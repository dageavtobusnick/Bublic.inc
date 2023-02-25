using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour,IPickable
{
    [SerializeField]
    private bool _inRange;
    [SerializeField]
    private int _price;
    [SerializeField]
    private List<GameObject> _itemsToSell;

    private GameObject _soldItem;
    private Text _priceText;
    private Text _coinsText;
    private CoinScore _coinScore;

    private void Start()
    {
        _soldItem = Instantiate(_itemsToSell[Random.Range(0, _itemsToSell.Count)], 
            transform.position, Quaternion.identity, transform);
        if (_soldItem.TryGetComponent<ObjectNameView>(out var nameView))
            nameView.enabled = false;
        if (_soldItem.TryGetComponent<PickUpRange>(out var pickUpRange))
            pickUpRange.DisableRange();
        if (_soldItem.TryGetComponent<CircleCollider2D>(out var circleCollider2D))
            circleCollider2D.enabled = false;
        if (_soldItem.TryGetComponent<BoxCollider2D>(out var boxCollider2D))
            boxCollider2D.enabled = false;
        if (_soldItem.TryGetComponent<PickableObject>(out var pickable))
            pickable.enabled = false;
        _coinScore = FindObjectOfType<CoinScore>();
        _coinsText = _coinScore.GetComponent<Text>();
        _priceText = GetComponentInChildren<Text>();
        _priceText.text = _price.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var _))
        {
            _priceText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var _))
        {
            _priceText.enabled = false;
        }
    }

    public void PickUp()
    {
        if (_coinScore.CoinCount >= _price)
        {
            _coinScore.SpendCoins(_price);
            GameController.Inventory.CollectItem(_soldItem);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(FlashCoins());
    }

    IEnumerator FlashCoins()
    {
        _coinsText.color = Color.red;
        yield return new WaitForSeconds(1);
        _coinsText.color = Color.white;
    }

}
