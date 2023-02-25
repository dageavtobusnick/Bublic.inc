using UnityEngine;
using System;

[RequireComponent(typeof(GUIBar))]
[RequireComponent(typeof(ColorController))]
public class HPBar : MonoBehaviour,IDamagable,IHealable,IEntity
{

    [SerializeField]
    private AudioClip _playerDamage;
    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private int _teamId;

    private int _currentHP;
    private bool _canBeDamaged=true;
    private AudioSource _playerAudioSource;

    public event Action Dead;
    public event Action Damaged;
    public event Action<float> HPChanged;
    public int CurrentHp { get => _currentHP; }
    public int MaxHp{ get => _maxHP;}
    public int TeamId { get => _teamId; }

    private void Start()
    {
        _currentHP = _maxHP;
        HPChanged?.Invoke(_currentHP);
        _playerAudioSource = GetComponent<AudioSource>();
        Dead += GameController.ReloadWorld;
        GetComponent<ColorController>().NotInv += () =>
        {
            _canBeDamaged = true;
        };
    }

    public void TakeDamage(int teamId,int damage)
    {
        if (_canBeDamaged&&teamId!=_teamId)
        {
            if (Time.timeScale == 0f)
            {
                _currentHP -= damage;
                if (_currentHP <= 1)
                    _currentHP = 1;
            }
            else
            {
                _currentHP -= damage;
                _playerAudioSource.PlayOneShot(_playerDamage);
                if (_currentHP <= 0)
                    Dead?.Invoke();
            }
            Damaged?.Invoke();
            _canBeDamaged = false;
            HPChanged?.Invoke(_currentHP);
        }
    }

    public void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
        HPChanged.Invoke(_currentHP);
    }

    public void BufHealth(int health)
    {
        _maxHP += health;
        Heal(health);
    }

    public void DebufHealth(int health)
    {
        _maxHP -= health;
        TakeDamage(GameController.VirtualTeamId,health);
    }

    public void InitHealth(int hp)
    {
        _maxHP = hp;
        _currentHP=hp;
    }
}
