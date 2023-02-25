using Pathfinding;
using UnityEngine;

public class SlimeLogic : MonoBehaviour,IAttacker
{
    private State _state;
    private GameObject _player;
    private Pathrooling _pathrooling;
    private AIPath _aIPath;
    private AIDestinationSetter _aIDestinationSetter;

    void Start()
    {
        _player = GameObject.Find("Player");
        _pathrooling = GetComponent<Pathrooling>();
        _pathrooling.LoadPoint(gameObject.transform.parent.gameObject);
        _aIDestinationSetter = GetComponent<AIDestinationSetter>();
        _player = FindObjectOfType<PlayerController>().gameObject;
        _aIPath = GetComponent<AIPath>();
        if (_pathrooling.CanPathrooling)
        {
            _state = State.Patrooling;
            _pathrooling.Patroling();
        }
    }

    void FixedUpdate()
    {
        if (_state == State.Patrooling && _aIPath.reachedDestination)
            _pathrooling.Patroling();
        _aIPath.enableRotation = true;
    }

    public void Attack()
    {
        _state = State.Atack;
        _aIDestinationSetter.target = _player.transform;
    }

    public void StopAttack()
    {
        _state = State.Patrooling;
        _pathrooling.Patroling();
    }
}
