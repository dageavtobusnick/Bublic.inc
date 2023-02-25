using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField]
    private int _ticks;
    [SerializeField]
    private int _tickDamage;
    [SerializeField]
    private int _timeBetweenTicksInSeconds;

    private HPBar _hP;
    private bool _poisonInflicted;

    void Start()
    {
        _hP = FindObjectOfType<HPBar>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var _) && !collision.isTrigger && !_poisonInflicted)
            StartCoroutine(InflictPoison());
    }

    IEnumerator InflictPoison()
    {
        _poisonInflicted = true;
        for (int i = 0; i < _ticks - 1; i++)
        {
            _hP.TakeDamage(GameController.VirtualTeamId,_tickDamage);
            yield return new WaitForSeconds(_timeBetweenTicksInSeconds);
        }
        _hP.TakeDamage(GameController.UndeadId, _tickDamage);
        _poisonInflicted = false;
    }
}
