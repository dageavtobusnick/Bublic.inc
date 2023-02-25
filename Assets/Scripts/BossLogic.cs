using Pathfinding;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Pathrooling))]
[RequireComponent(typeof(HP))]
[RequireComponent (typeof(AIPath))]
[RequireComponent(typeof (AIDestinationSetter))]
public class BossLogic : MonoBehaviour,IAttacker
{
    [SerializeField]
    private GameObject _laserGroup1;
    [SerializeField]
    private GameObject _laserGroup2;
    [SerializeField]
    private GameObject _laserGroup3;
    [SerializeField]
    private float _firstStageAtackKD;
    [SerializeField]
    private float _turboSpeed;
    [SerializeField]
    private float _normalSpeed;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private float _secondStageWaitingOnPoint;
    [SerializeField]
    private float _distanseNoFlySpawn;
    [SerializeField]
    private float _minFlySpawn;
    [SerializeField]
    private GameObject _fly;
    [SerializeField]
    private float _runningAwayDistance;

    private BossStage _bossStage = BossStage.Sleep;
    private BossFirstStage _firstStage = BossFirstStage.None;
    private GameObject _centerPoint;
    private Transform _player;
    private GameObject[] _flyingPoints;
    private GameObject[] _flySpawnPoints;
    private BossThirdStage _bossThirdStage=BossThirdStage.Atack;
    private float _waitingOnPoint=0;
    private float _firstStageAtackTime = 0;
    private bool _isWaitingOnPoint = false;
    private HP _hP;
    private Pathrooling _pathrooling;
    private Transform _transform;
    private Animator _animator;
    private AIPath _aIPath;
    private AIDestinationSetter _destinationSetter;
    private SecondStageController _secondStageController;
    private BossMainShootSystem _lG1System;
    private BossMainShootSystem _lG2System;
    private BossMainShootSystem _lG3System;



    void Start()
    {
        _transform = GetComponent<Transform>();
        _pathrooling = GetComponent<Pathrooling>();
        _aIPath = GetComponent<AIPath>();
        _destinationSetter= GetComponent<AIDestinationSetter>();
        _secondStageController = GetComponentInParent<SecondStageController>();
        _pathrooling.LoadPoint(gameObject.transform.parent.gameObject);
        _flyingPoints = GameObject.FindGameObjectsWithTag("flypoint").Where(x => x.transform.parent.gameObject == gameObject.transform.parent.gameObject).ToArray();
        _flySpawnPoints = GameObject.FindGameObjectsWithTag("flyspawn");
        _centerPoint = GameObject.FindGameObjectWithTag("center");
        _hP = gameObject.GetComponent<HP>();
        _player = FindObjectOfType<PlayerController>().transform;
        _destinationSetter.target = _centerPoint.transform;
        _aIPath.canSearch = false;
        _animator = GetComponent<Animator>();
        _shield.GetComponent<SpriteRenderer>().enabled = false;
        InitShootSystem(_laserGroup1);
        InitShootSystem(_laserGroup2);
        InitShootSystem(_laserGroup3);
        _lG1System = _laserGroup1.GetComponent<BossMainShootSystem>();
        _lG2System = _laserGroup2.GetComponent<BossMainShootSystem>();
        _lG3System = _laserGroup3.GetComponent<BossMainShootSystem>();
    }

    private void InitShootSystem(GameObject shootCluster)
    {
        var l1 = shootCluster.GetComponentsInChildren<BossShootSystem>();

        foreach (var e in l1)
        {
            e.WeaponStats = GetComponent<WeaponStats>();
        }
    }

    void FixedUpdate()
    {
        Animate();
        StageCheck();
        FirstStage();
        SecondStage();
        ThirdStage();
    }

    private void FirstStage()
    {
        if (_bossStage == BossStage.First && _aIPath.reachedDestination)
        {
              _pathrooling.Patroling();
        }
        if (_bossStage == BossStage.First)
        {
            _firstStageAtackTime += Time.fixedDeltaTime;
        }
        if (_firstStageAtackTime >= _firstStageAtackKD)
        {
            _firstStageAtackTime = 0;
            _bossStage = BossStage.FirstStageAtack;
            _laserGroup1.SetActive(true);
            _lG1System.StartShooting();
            _destinationSetter.target = _centerPoint.transform;
            _aIPath.maxSpeed = _turboSpeed;
            _firstStage = BossFirstStage.First;
            gameObject.tag = "Untagged";
            _shield.SetActive(true);
        }
        if (_bossStage == BossStage.FirstStageAtack)
        {
            FirstStageShooterPhase();
        }
    }

    private void SecondStage()
    {
        if (_bossStage == BossStage.Second && !_isWaitingOnPoint)
        {
            var point = _flyingPoints[UnityEngine.Random.Range(0, _flyingPoints.Length - 1)];
            gameObject.transform.position = point.transform.position;
            _destinationSetter.target = point.transform;
            _isWaitingOnPoint = true;
            _waitingOnPoint = 0;
        }
        if (_isWaitingOnPoint && _waitingOnPoint >= _secondStageWaitingOnPoint && _bossStage == BossStage.Second)
        {
            _isWaitingOnPoint = false;
            if (Math.Abs((_player.position - gameObject.transform.position).magnitude) >= _distanseNoFlySpawn)
            {
                SpawnFly();
                _secondStageController.StartStage();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (_isWaitingOnPoint && _bossStage == BossStage.Second)
            {
                _waitingOnPoint += Time.fixedDeltaTime;
            }
        }
    }

    private void ThirdStage()
    {
        if (_bossStage == BossStage.Third)
        {
            _destinationSetter.target = _player;
            _aIPath.maxSpeed = _turboSpeed;
        }
        if (_bossThirdStage == BossThirdStage.Atack && _bossStage == BossStage.Third && (_player.position - gameObject.transform.position)
                                                                                      .sqrMagnitude <= _runningAwayDistance * _runningAwayDistance)
        {
            if (_flyingPoints.Length != 0)
                _destinationSetter.target = _flyingPoints[UnityEngine.Random.Range(0, _flyingPoints.Length)].transform;
            _bossThirdStage = BossThirdStage.MoveToPoint;
        }
        else if (_bossThirdStage == BossThirdStage.MoveToPoint && _bossStage == BossStage.Third && _aIPath.reachedEndOfPath)
        {
            _destinationSetter.target = _player;
            _bossThirdStage = BossThirdStage.Atack;
        }
    }

    private void FirstStageShooterPhase()
    {
        if (!_lG1System.IsShooting() && _firstStage == BossFirstStage.First)
        {
            _laserGroup1.SetActive(false);
            _laserGroup2.SetActive(true);
            _lG2System.StartShooting();
            _firstStage = BossFirstStage.Second;
        }
        if (!_lG2System.IsShooting() && _firstStage == BossFirstStage.Second)
        {
            _laserGroup2.SetActive(false);
            _laserGroup3.SetActive(true);
            _lG3System.StartShooting();
            _firstStage = BossFirstStage.Third;
        }
        if (!_lG3System.IsShooting() && _firstStage == BossFirstStage.Third)
        {
            _laserGroup3.SetActive(false);
            _aIPath.maxSpeed = _normalSpeed;
            _bossStage = BossStage.First;
            _shield.SetActive(false);
            _firstStage = BossFirstStage.None;
            gameObject.tag = "Enemy";
        }
    }

    public void Animate()
    {
        var playerPos = GameController.Player.transform.position;
        var enemyPos = _transform.position;
        var dX = playerPos.x - enemyPos.x;
        var dY = playerPos.y - enemyPos.y;
        _animator.SetFloat("dX", dX);
        _animator.SetFloat("dY", dY);
        _animator.SetBool("Shielded", _shield.activeInHierarchy);
    }

    private void SpawnFly()
    {
        if (_minFlySpawn >= _flySpawnPoints.Length)
            return;
        var count = 0;
        var spawnFly = UnityEngine.Random.Range(_minFlySpawn, _flySpawnPoints.Length);
        foreach (var e in _flySpawnPoints)
        {
            if (count >= spawnFly)
                break;
            var newFly = Instantiate(_fly, e.transform.position, Quaternion.identity);
            newFly.transform.parent = _transform.parent;
            count++;
        }
    }


    private void StageCheck()
    {
        var hpPersent = (_hP.CurrentHp*1f / _hP.MaxHp) * 100;
        if (hpPersent <= 70 && _bossStage == BossStage.First)
        {
            _bossStage = BossStage.Second;
        }
        if (hpPersent <= 30 && _bossStage == BossStage.Second)
        {
            _bossStage = BossStage.Third;
        }
    }

    public void Attack()
    {
        _bossStage = BossStage.First;
        _aIPath.slowWhenNotFacingTarget = false;
        _aIPath.canSearch = true;
    }

    public void StopAttack()
    {
        _bossStage = BossStage.Sleep;
        _aIPath.slowWhenNotFacingTarget = true;
        _aIPath.canSearch = false;
    }
}
public enum BossStage
{
    Sleep,
    First,
    FirstStageAtack,
    Second,
    Third
}
public enum BossFirstStage
{
    First,
    Second,
    Third,
    None
}

public enum BossThirdStage
{
    Atack,
    MoveToPoint
}
