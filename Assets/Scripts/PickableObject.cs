using UnityEngine;

public class PickableObject : MonoBehaviour,IPickable
{
    private bool _isPicked=false;

    public bool IsPicked { get => _isPicked;}

    public virtual void PickUp()
   {
        if (!_isPicked)
        {
            if (TryGetComponent<SpriteRenderer>(out var renderer))
                renderer.enabled = false;
            if (TryGetComponent<ObjectsMove>(out var objectAnimator))
            {
                objectAnimator.PickUp();
                objectAnimator.enabled = false;
            }
            if (TryGetComponent<ObjectNameView>(out var name))
                name.PickUp();
            GameController.Inventory.CollectItem(gameObject);
            _isPicked = true;
        }
    }
}
