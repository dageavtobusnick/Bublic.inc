using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _weapons;
    [SerializeField]
    private GameObject _canvas;

    private bool _isClose = true;
    private PlayerInputActions _actions;

    private void Start()
    {
        _actions = GameController.PlayerInputActions;
    }

    public void DropWeapon()
    {
        GameObject weapon = Instantiate(_weapons[Random.Range(0, _weapons.Count)], transform.position, Quaternion.identity) ;
        weapon.transform.position = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_actions.IsTryCollect && TryGetComponent<PlayerController>(out var _) && _isClose)
        {
            _isClose = false;
            DropWeapon();
            _canvas.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isClose)
            _canvas.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canvas.SetActive(false);
    }

}
