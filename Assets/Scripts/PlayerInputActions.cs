using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    private Vector2 _axis;
    private bool _isTryCollect;

    public Vector2 Axis { get => _axis; }
    public bool IsTryCollect { get => _isTryCollect; }

    public event Action Fire;

    public event Action Dash;

    public void OnMove(InputAction.CallbackContext context)
    {
        _axis=context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
            Fire?.Invoke();
    }

    public void OnTryCollect(InputAction.CallbackContext context)
    {
        _isTryCollect=context.performed;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
            Dash?.Invoke();
    }

}
