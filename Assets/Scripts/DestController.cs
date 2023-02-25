using UnityEngine;

public class DestController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var _))
            GetComponentInParent<MoskitoController>().TheeToFour();     
    }
}
