using UnityEngine;

public class ObjectsMove : MonoBehaviour
{
    [SerializeField]
    private float _amp=0.05f;
    [SerializeField]
    private float _freq=2;

    private bool _isPicked = false;
    private Vector2 _startPos;
    private float _time = 0;
    private float _offset = 0;

    private Transform _transform;
    void Start()
    {
        _transform = transform;
        _startPos = _transform.position;
    }

    public void PickUp()
    {
        _isPicked = true;
    }

    void FixedUpdate()
    {
        if (!_isPicked)
        {
            _time += Time.fixedDeltaTime;
            _offset = _amp * Mathf.Sin(_time * _freq);
            _transform.position = _startPos + new Vector2(0, _offset);
        }
    }
}
