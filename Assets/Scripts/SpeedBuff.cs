using UnityEngine;

public class SpeedBuff : MonoBehaviour, IModuleEffect
{
    [SerializeField]
    private float _speedIncrease;

    public void ActivateEffect(bool activate) =>
        GameController.Player.Speed += (activate ? _speedIncrease : -_speedIncrease);
}
