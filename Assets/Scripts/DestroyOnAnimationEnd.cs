using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void OnAnimationEnded()
    {
        Destroy(gameObject);
    }
}
