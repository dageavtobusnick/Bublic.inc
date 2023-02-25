using UnityEngine;

[RequireComponent(typeof(HP))]
public class DeleateOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToDelete;
    void Start()
    {
        GetComponent<HP>().Dead += OnDeath;
    }

    private void OnDeath()
    {
        GameController.CurrentRoom.RemoveEnemy(_objectToDelete);
    }
}
