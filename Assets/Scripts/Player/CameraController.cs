using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }
    void LateUpdate()
    {
        _transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, _transform.position.z);
    }
}
