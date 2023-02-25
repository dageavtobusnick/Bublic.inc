using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InventoryScript _inv;
    [SerializeField]
    private Canvas _canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _inv.Use(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _inv.Use(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _inv.Use(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _inv.Use(3);
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_canvas.enabled)
            {
                _canvas.enabled = false;
                Time.timeScale = 1;
            }
            else
            {
                _canvas.enabled = true;
                Time.timeScale = 0;
            }
        }
    }
}
