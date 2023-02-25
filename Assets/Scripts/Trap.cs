using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int _damage = 35;
    [SerializeField] private float _currentCD = 0;
    [SerializeField] private float _trapCD = 1f;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (_currentCD <= 0)
        {
            collision.GetComponent<IDamagable>().TakeDamage(GameController.UndeadId, _damage);
            _currentCD = _trapCD;
        }
        else
        {
            _currentCD -= Time.deltaTime;
        }
    }
}
