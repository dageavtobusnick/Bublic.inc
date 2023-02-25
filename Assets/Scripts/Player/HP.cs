using System;
using UnityEngine;

[RequireComponent(typeof(GUIBar))]
[RequireComponent(typeof(ColorController))]
public class HP : MonoBehaviour,IDamagable,IEntity
{
    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private int _teamId;
    [SerializeField]
    private AudioClip _damageSound;

    private bool _canBeDamaged=true;
    private int _currentHP;
    private AudioSource _playerAudioSource;

    public event Action Dead;
    public event Action Damaged;
    public event Action<float> HPChanged;

    public int CurrentHp { get => _currentHP; }

    public int MaxHp { get => _maxHP; }
    public int TeamId { get => _teamId; }

    private void Start()
    {
        _currentHP = _maxHP;
        _playerAudioSource = GetComponent<AudioSource>();
        GetComponent<ColorController>().NotInv += () =>
        {
            _canBeDamaged = true;
        };
    }


    public void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
        HPChanged.Invoke(_currentHP);
    }

    public void Die()
    {
        Dead?.Invoke();
    }

    public void TakeDamage(int teamId, int damage)
    {
        if (_canBeDamaged&&teamId!=_teamId)
        {
            _currentHP -= damage;
            _playerAudioSource.PlayOneShot(_damageSound);
            Damaged?.Invoke();
            HPChanged?.Invoke(_currentHP);
            if (_currentHP <= 0)
                Die();
            _canBeDamaged=false;
        }
    }

    public void InitHealth(int hp)
    {
        _maxHP = hp;
        _currentHP = hp;
    }
}
