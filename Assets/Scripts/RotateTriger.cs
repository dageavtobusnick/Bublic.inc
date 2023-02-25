using UnityEngine;

public class RotateTriger : MonoBehaviour
{
    private Rotator rotator;
    private void Start()
    {
        rotator = transform.parent.gameObject.GetComponentInChildren<Rotator>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var _))
            rotator.StartRotate();
    }
}
