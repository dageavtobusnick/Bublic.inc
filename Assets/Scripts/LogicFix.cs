using UnityEngine;

public class LogicFix : MonoBehaviour
{
    private Collider2D Collider2D;
    private float timer=7;

    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (Collider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if (timer <= 0)
                Collider2D.isTrigger = true;
            else timer -= Time.fixedDeltaTime;
        }
        else {
            timer = 7;
            Collider2D.isTrigger = false;
        }
    }
}
