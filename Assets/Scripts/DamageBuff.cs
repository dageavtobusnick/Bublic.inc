using UnityEngine;

public class DamageBuff : MonoBehaviour, IModuleEffect
{
    [SerializeField]
    private int _damageIncrease;

    public void ActivateEffect(bool activate)=> 
        GameController.DamageBonus +=(activate?_damageIncrease:-_damageIncrease);

}
