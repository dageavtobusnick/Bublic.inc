using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(IDamagable))]
public class VaseScript : MonoBehaviour
{
    [System.Serializable]
    public class LootChances
    {
        public GameObject Loot;
        public int Chance;
    }
    [SerializeField]
    private List<LootChances> _lootChances;

    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        GetComponent<IDamagable>().Dead += Break;
    }

    public void Break()
    {
        var rand = Random.Range(0 + GameController.LuckBonus, 100 - GameController.LuckBonus * 2);

        for (int i = 0; i < _lootChances.Count; i++)
            if (rand < _lootChances[i].Chance)
            {
                Instantiate(_lootChances[i].Loot,
                            new Vector3(transform.position.x, transform.position.y - 0.25f, 4),
                            Quaternion.identity);
                break;
            }

        _capsuleCollider.enabled = false;
        _animator.SetTrigger("Broke");
    }
}
