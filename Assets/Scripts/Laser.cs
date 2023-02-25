using UnityEngine;

public class Laser : Bullet
{
    [SerializeField]
    private float _liveTime=10;
    private float _totalLiveTime = 0;

    private void Update()
    {
        _totalLiveTime += Time.deltaTime;
        if (_totalLiveTime >= _liveTime)
        {
            Destroy(gameObject);
        }
    }
}
