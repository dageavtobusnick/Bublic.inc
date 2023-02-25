using UnityEngine;

public class Rotator : MonoBehaviour
{
    private bool _canRotate = false;
    private float _rotateTimer;
    private float _kDtimer;

    private JagerStats _jagerStats;
    private Transform _transform;
    public bool IsRotating { get; private set; }

    private void Start()
    {
        _jagerStats = GetComponentInParent<JagerStats>();
        _transform = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if (IsRotating)
        {
            if (_rotateTimer <= 0)
            {
                IsRotating = false;
                _kDtimer = _jagerStats.AtackKD;
            }
            else
            {
                _transform.Rotate(0f, 0f,-_jagerStats.RotateSpeed*1000 * Time.fixedDeltaTime);
                _rotateTimer -= Time.fixedDeltaTime;
            }
        }
        if (!IsRotating && !_canRotate )
        {   if (_kDtimer > 0)
                _kDtimer -= Time.fixedDeltaTime;
            else
                _canRotate = true;
        }
    }
    public void StartRotate()
    {
        if (_canRotate)
        {
            var stats = GetComponentInParent<JagerStats>();
            _rotateTimer = stats.AtackTime;
            IsRotating = true;
            _canRotate = false;
        }
    }
}
