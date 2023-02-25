using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _speed = 5f;
    [SerializeField]
    private float _dashCD = 1f;
    [SerializeField]
    private float _dashSpeed = 1;
    [SerializeField]
    private float _dashTime = 1.0f;
    [SerializeField]
    private AudioClip _coinsSound;
    [SerializeField]
    private Collider2D _hitbox;

	private bool _isDashing;
    private Rigidbody2D _rb;
    private float _currentCD = 0;
    private Animator _animator;
	private SpriteRenderer _playerSprite;

    public float Speed { get => _speed; set => _speed = value; }

    public event Action OnCoinTake;

    void Start()
	{
		_playerSprite = GetComponent<SpriteRenderer>();
		_hitbox = GetComponent<BoxCollider2D>();
		_rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_rb.freezeRotation = true;
		_rb.gravityScale = 0;
		GameController.PlayerInputActions.Dash += OnDash;
	}

	void FixedUpdate()
	{
		var move = GameController.PlayerInputActions.Axis;
		_rb.velocity = move.normalized *( (_isDashing)? _dashSpeed: _speed);
        SetPlayerDirection();
    }

	void Update()
	{
		if (Time.timeScale == 0f || _isDashing) return;
		if (_currentCD > 0)
			_currentCD -= Time.deltaTime;
	}

	public void OnDash()
	{
        if ( !_isDashing && _currentCD <= 0)
        {
            StartCoroutine(DashCoroutine());
            _isDashing = true;
            _currentCD = _dashCD;
        }
    }

	private IEnumerator DashCoroutine()
	{
		_playerSprite.color = new Color(1, 1, 1, 0.5f);
		_hitbox.enabled = false;
		float startTime = Time.time;
		while (Time.time < startTime + _dashTime)
		{
			yield return null;
		}
		_playerSprite.color = new Color(1, 1, 1, 1f);
		_hitbox.enabled = true;
		_isDashing = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            TakeCoin();
            Destroy(collision.gameObject);
        }
    }

    private void TakeCoin()
    {
        OnCoinTake?.Invoke();
        gameObject.GetComponent<AudioSource>().PlayOneShot(_coinsSound);
    }

	void SetPlayerDirection()
    {
		var movementVector = GameController.PlayerInputActions.Axis.normalized;
		_animator.SetFloat("X", movementVector.x);
		_animator.SetFloat("Y", movementVector.y);
    }
}
