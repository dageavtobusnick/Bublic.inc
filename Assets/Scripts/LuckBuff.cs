using UnityEngine;

public class LuckBuff : MonoBehaviour, IModuleEffect
{
    [SerializeField]
    private int _luckIncrease;

    public void ActivateEffect(bool activate) => 
        GameController.LuckBonus += (activate ? _luckIncrease : -_luckIncrease);

}
