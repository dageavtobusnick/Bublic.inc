using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PickUpRange))]
public class MeleeWeapon : PickableObject
{
    [SerializeField] 
    private int _damage;
    [SerializeField] 
    private float _attackCooldown;
    [SerializeField]
    private float _distanceOffset;
    [SerializeField]
    private float _angleOffset;
    [SerializeField]
    private AudioClip _attackSound;
    [SerializeField]
    private int _teamId=0;

    private PickUpRange _range;
    private PolygonCollider2D _collider;
    private Transform _playerTransform;
    private Transform _transform;
    private Transform _camTransform;
    private AudioSource _audioSource;
    private PlayerInputActions _actions;
    private Animator _animator;
    private SpriteRenderer _weaponSprite;
    private float _kD;

    void Start()
    {
        _weaponSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _range=GetComponent<PickUpRange>();
        _collider = GetComponent<PolygonCollider2D>();
        _playerTransform = GameController.Player.transform;
        _transform = GetComponent<Transform>();
        _camTransform = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
        _actions = GameController.Player.GetComponent<PlayerInputActions>();
    }


    void Update()
    {
        if (IsPicked && Time.timeScale > 0)
        { 
            if (_kD > 0)
            {
                _kD -= Time.deltaTime;
                if (_kD <= 0)
                    StartCoroutine(Blink());
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                _collider.enabled = true;
            else
                _collider.enabled = false;
        }

    }

    public void Atack()
    {
        if (_kD <= 0)
        {
            _weaponSprite.color = Color.white;
            DoDamage();
            _audioSource.PlayOneShot(_attackSound);
            _kD = _attackCooldown;
        }
    }

    public void UpdateWeaponTeam(int teamId)
    {
        _teamId = teamId;
    }

    public void FixedUpdate()
    {
        if (IsPicked && Time.timeScale > 0)
        {
            LookAtCursor();
        }
    }

    public IEnumerator Blink()
    {
        _weaponSprite.color = new Color(0.18f, 0.3f, 0.3f);
        yield return new WaitForSeconds(0.25f);
        _weaponSprite.color = Color.white;
    }

    public override void PickUp()
    {
        if (!IsPicked)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            _range.DisableRange();
            base.PickUp();
        }
    }

    private void LookAtCursor()
    {
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,_camTransform.position.z));
        lookPos -= _playerTransform.position;
        lookPos.z = 0;

        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.AngleAxis(angle - _angleOffset, Vector3.forward);
        var desiredPosition = _playerTransform.position + lookPos.normalized * _distanceOffset;
        _transform.position = Vector3.MoveTowards(_transform.position, desiredPosition, Time.fixedDeltaTime* 80f);
    }

    public void Equip()
    {
        gameObject.SetActive(true);
        _collider.enabled = true;
        _weaponSprite.enabled = true;
        _actions.Fire += Atack;
    }

    public void Deequip()
    {
        gameObject.SetActive(false);
        _collider.enabled = false;
        _weaponSprite.enabled = false;
        _actions.Fire -= Atack;
    }

    public void DoDamage()
    {
        _animator.SetTrigger("Attacked");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HP>(out var hP))
            hP.TakeDamage(_teamId,_damage + GameController.DamageBonus);
    }
}
