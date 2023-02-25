using UnityEngine;

[RequireComponent(typeof(IDamagable))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToDestroy;
    void Start()
    {
        GetComponent<IDamagable>().Dead += DestroyObject;
    }

    public void DestroyObject()
    {
        Destroy(_objectToDestroy);
    }
}
