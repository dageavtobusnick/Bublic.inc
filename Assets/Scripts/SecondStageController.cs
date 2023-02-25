using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    [SerializeField]
    private GameObject _boss;
    [SerializeField]
    private int _damageInPersent;

    private bool _isStarted=false;
    private HP _hP;

    private void Start()
    {
        _hP=_boss.GetComponent<HP>();
    }

    public void StartStage()
    {
        _isStarted = true;
    }
    void FixedUpdate()
    {
        if (_isStarted)
        {
            var flyes = gameObject.GetComponentsInChildren<IAttacker>();
            if (flyes.Length == 0)
            {
                _isStarted = false;
                _boss.SetActive(true);
                _hP.TakeDamage(GameController.VirtualTeamId,_hP.MaxHp * _damageInPersent / 100);
            }
            else
            {
                foreach(var e in flyes)
                {
                    e.Attack();
                }
            }
        }
        
    }
}
