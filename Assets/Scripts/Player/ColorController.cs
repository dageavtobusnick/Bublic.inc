using System;
using UnityEngine;

[RequireComponent(typeof(IDamagable))]
public class ColorController : MonoBehaviour
{
    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _damagedColor;
    [SerializeField]
    private float _invFrames;

    private float _currentInvTime;
    private SpriteRenderer _sprite;

    public event Action NotInv;
    void Start()
    {
        GetComponent<IDamagable>().Damaged += OnDamaged;
        _sprite=GetComponent<SpriteRenderer>();
    }

    private void OnDamaged()
    {
        if (_currentInvTime == 0f)
        {
            _currentInvTime = _invFrames;
            _sprite.color = _damagedColor;
        }
    }
    private void FixedUpdate()
    {
        if (_currentInvTime > 0)
        {
            _currentInvTime -= Time.fixedDeltaTime;

            if (_currentInvTime <= 0f)
            {
                _currentInvTime = 0f;
                _sprite.color = _normalColor;
                NotInv?.Invoke();
            }
        }
    }
}
