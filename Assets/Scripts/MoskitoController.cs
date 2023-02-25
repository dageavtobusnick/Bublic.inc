using Pathfinding;
using System.Linq;
using UnityEngine;

public enum MoskitoStages
{
    One,
    Two,
    Three,
    Four,
    Five
}
public class MoskitoController : MonoBehaviour
{
    [SerializeField]
    private int _hP;
    [SerializeField]
    private float _flyRange;
    [SerializeField]
    private float _flyingSpeed;
    [SerializeField]
    private float _atackRange;
    [SerializeField]
    private float _fastAtackSpeed;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _kD;
    [SerializeField]
    private float _moveToPositionSpeed;
    [SerializeField]
    private float _attackStageTime;

    private MoskitoStages _stages = MoskitoStages.One;
    private AIPath _aIPath;
    private AIDestinationSetter _aIDestinationSetter;
    private float _moveToTargetTimer = 0;
    private PlayerController _player;
    private float _atackTimer;
    private Vector3 _vel;
    private float _kDTimer;
    private bool _canAtack=true;
    private GameObject[] _positions;
    private Transform _transform;
    void Start()
    {
        GetComponentInChildren<IDamagable>().InitHealth(_hP);
        GetComponentInChildren<ContactDamage>().InitDamage(_damage);
        transform.GetChild(0).localPosition = new Vector3(0, _flyRange, 0);
        _aIPath = gameObject.GetComponentInChildren<AIPath>();
        _aIDestinationSetter = gameObject.GetComponentInChildren<AIDestinationSetter>();
        _player = GameController.Player;
        _transform = transform;
        _positions = GameObject.FindGameObjectsWithTag("position").Where(x => x.transform.parent.gameObject ==
                                                                         _transform.parent.gameObject).ToArray();
        GetComponent<CircleCollider2D>().radius = _atackRange;
    }

    void FixedUpdate()
    {
        FirstStage();
        SecondStage();
        if (_stages == MoskitoStages.Three)
        {
            _moveToTargetTimer += Time.fixedDeltaTime;
            _transform.position += _vel * Time.fixedDeltaTime;
        }
        FourthStage();
        if (_stages == MoskitoStages.Five)
        {
            if (_aIPath.reachedEndOfPath)
            {
                _transform.GetChild(0).localPosition = new Vector3(0, _flyRange, 0);
                _stages = MoskitoStages.One;
                _kDTimer = _kD;
            }
        }
    }

    private void FourthStage()
    {
        if (_stages == MoskitoStages.Four || _stages == MoskitoStages.Three)
        {
            if (_atackTimer <= 0)
            {
                _stages = MoskitoStages.Five;
                _moveToTargetTimer = 0;
                _atackTimer = 0;
                MoveToPosition();
            }
            else _atackTimer -= Time.fixedDeltaTime;
        }
        if (_stages == MoskitoStages.Four)
        {
            _transform.position += _vel * Time.fixedDeltaTime;
            if (_moveToTargetTimer <= 0)
            {
                _moveToTargetTimer = 0;
                _stages = MoskitoStages.Five;
                MoveToPosition();
            }
            else _moveToTargetTimer -= Time.deltaTime;
        }
    }

    private void SecondStage()
    {
        if (_stages == MoskitoStages.Two)
        {
            _stages = MoskitoStages.Three;
            _transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            _vel = (_player.transform.position - _transform.position).normalized * _fastAtackSpeed;
            _atackTimer = _attackStageTime;
        }
    }

    private void FirstStage()
    {
        if (_stages == MoskitoStages.One)
        {
            _transform.Rotate(0, 0, -_flyingSpeed * Time.fixedDeltaTime);
            _transform.GetChild(0).Rotate(0, 0, _flyingSpeed * Time.fixedDeltaTime);
            if (!_canAtack)
            {
                if (_kDTimer <= 0)
                {
                    _canAtack = true;
                    _kDTimer = _kD;
                }
                else _kDTimer -= Time.fixedDeltaTime;
            }
        }
    }

    void StartAtack()
    {
        if (_stages == MoskitoStages.One&&_canAtack)
        {
            _stages = MoskitoStages.Two;
            _canAtack = false;
        }
    }
    public void TheeToFour()
    {
        if (_stages == MoskitoStages.Three)
        {
            _stages = MoskitoStages.Four;
            _atackTimer = _attackStageTime;
        }
    }

    void MoveToPosition()
    {
        var position = _positions.OrderBy(x => (x.transform.position - _transform.position).sqrMagnitude).First();
        _aIDestinationSetter.target = position.transform;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (TryGetComponent<PlayerController>(out var _))
            StartAtack();
        if (collision.CompareTag("wall"))
        {
            _stages = MoskitoStages.Five;
            _atackTimer = 0;
            _moveToTargetTimer = 0;
        }
    }
}
