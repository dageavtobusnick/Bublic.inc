using UnityEngine;

public class PickUpRange : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _range;

    public void EnableRange() => _range.enabled = true;
    public void DisableRange() => _range.enabled = false;
}
