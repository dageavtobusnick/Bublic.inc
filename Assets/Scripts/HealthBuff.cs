using UnityEngine;

public class HealthBuff : MonoBehaviour, IModuleEffect
{
    [SerializeField]
    private int _healthIncrease;

    private HPBar _playerHealth;

    public void Awake()
    {
        _playerHealth = GetComponent<HPBar>();
    }
    public void ActivateEffect(bool activate)
    {
        if (activate)
            _playerHealth.BufHealth(_healthIncrease);
        else
            _playerHealth.DebufHealth(_healthIncrease);
    }
}
