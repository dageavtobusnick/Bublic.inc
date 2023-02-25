using UnityEngine;

[RequireComponent(typeof(IDamagable))]
public class GUIBar : MonoBehaviour
{
    [SerializeField]
    private Color _textColor = Color.white;
    [SerializeField]
    private float _textHeight = 0.8f;

    private GUIStyle _style = new();
    private IDamagable _damagable;

    private void Awake()
    {
        GUI.depth = 9999;
        _style = new();
        _style.normal.textColor = _textColor;
    }

    private void Start()
    {
        _damagable=GetComponent<IDamagable>();
    }


    void OnGUI()
    {
        Vector3 worldPosition = new(transform.position.x, transform.position.y + _textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;
        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), _damagable.CurrentHp.ToString(), _style);
    }
}
