using Pathfinding;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Pathrooling))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class FlyLogic : MonoBehaviour,IAttacker
{
    private AIPath _aIPath;
    private State _state;
    private Pathrooling _pathrooling;
    private AIDestinationSetter _aIDestinationSetter;
    private Transform[] _flyingPoints;
    private GameObject _player;
    private Collider2D _collider;
    private Collider2D _playerCollider;

    void Start()
    {
        _flyingPoints = GameObject.FindGameObjectsWithTag("flypoint")
            .Where(x => x.transform.parent.gameObject == gameObject.transform.parent.gameObject)
            .Select(x => x.transform).ToArray();
        _player = FindObjectOfType<PlayerController>().gameObject;
        _pathrooling = GetComponent<Pathrooling>();
        _pathrooling.LoadPoint(gameObject.transform.parent.gameObject);
        _state = State.Stop;
        _aIPath= GetComponent<AIPath>();
        _aIDestinationSetter=GetComponent<AIDestinationSetter>();
        _collider = GetComponent<Collider2D>();
        _playerCollider = _player.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (_state == State.Patrooling && _aIPath.reachedDestination)
            _pathrooling.Patroling();
        _aIPath.enableRotation = true;
        if (_state == State.Atack && _collider.IsTouching(_playerCollider))
        {
            if (_flyingPoints.Length != 0)
                _aIDestinationSetter.target = _flyingPoints[Random.Range(0, _flyingPoints.Length)];
            _state = State.MoveToPoint;
        }
        else if (_state == State.MoveToPoint && _aIPath.reachedEndOfPath)
        {
            _aIDestinationSetter.target = _player.transform;
            _state = State.Atack;
        }
    }

    public void Attack()
    {
        _state = State.Atack;
        _aIDestinationSetter.target = _player.transform;
    }

    public void StopAttack()
    {
        _state = State.Stop;
    }
}
