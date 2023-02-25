using Pathfinding;
using UnityEngine;

public class SkeletonLogic : MonoBehaviour,IAttacker
{
    [SerializeField]
    private AIPath _aIPath;
    [SerializeField]
    private AIDestinationSetter _aIDestinationSetter;
    [SerializeField]
    private float _distanceToPlayer;
    [SerializeField]
    private float _baseDistance;
    [SerializeField]
    private float _runningAwayDistance = 3.2f;
    [SerializeField]
    private float _obstacleWT;
    [SerializeField]
    private Transform _scaringZone;

    private State _state;
    private GameObject _player;
    private Animator _animator;
    private Transform _transform;
    private Pathrooling _pathrooling;
    private PolygonCollider2D _collider;
    private Collider2D _playerCollider;
    public void Attack()
    {
        gameObject.GetComponent<ShootSystem>().IsShooting = true;
        _aIDestinationSetter.target = _player.transform;
        LookAtPlayer();
    }

    public void StopAttack()
    {
        _state = State.Patrooling;
        _pathrooling.Patroling();
    }

    private void LookAtPlayer()
    {
        var playerPos = _aIDestinationSetter.target.position;
        var enemyPos = _transform.position;
        var dX = playerPos.x - enemyPos.x;
        var dY = playerPos.y - enemyPos.y;
        _animator.SetFloat("dX", dX);
        _animator.SetFloat("dY", dY);
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>().gameObject;
        _transform = GetComponent<Transform>();
        _pathrooling = GetComponent<Pathrooling>();
        _scaringZone.localScale = Vector2.one * _runningAwayDistance;
        _pathrooling.LoadPoint(gameObject.transform.parent.parent.gameObject);
        if (_pathrooling.CanPathrooling)
        {
            _state = State.Patrooling;
            _pathrooling.Patroling();
        }
        _collider = gameObject.GetComponentInChildren<PolygonCollider2D>();
        _playerCollider = _player.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (_state == State.Patrooling && _aIPath.reachedDestination)
                _pathrooling.Patroling();
        if (_state == State.Atack || _state == State.MoveToPoint||_state==State.Patrooling)
        {
            LookAtPlayer();
        }
        if (_state == State.Atack && _collider.IsTouching(_playerCollider))
        {
            _state = State.MoveToPoint;
            var point = _pathrooling.GetSpecialPoint(x => (x.transform.position - _player.transform.position).sqrMagnitude > _runningAwayDistance * _runningAwayDistance,
                                                     x => Mathf.Abs((x.transform.position - gameObject.transform.position).magnitude));
            _aIDestinationSetter.target = point.transform;
            _aIPath.endReachedDistance = _baseDistance;
        }
        else if (_state == State.MoveToPoint && !_collider.IsTouching(_playerCollider))
        {
            _aIDestinationSetter.target = _player.transform;
            _state = State.Atack;
            _aIPath.endReachedDistance = _distanceToPlayer;
        }
    }
}
