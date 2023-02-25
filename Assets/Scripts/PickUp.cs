using UnityEngine;

public class PickUp : MonoBehaviour
{
    private PlayerInputActions _actions;

    private void Start()
    {
        _actions = GetComponent<PlayerInputActions>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_actions.IsTryCollect&&collision.TryGetComponent<IPickable>(out var pickable))
        {
            pickable.PickUp();
        }
    }
}
