using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour
{
    [SerializeField]
    private int _healAmount;
    public void Heal()
    {
        GameController.Player.GetComponent<IHealable>().Heal(_healAmount);
    }
}
