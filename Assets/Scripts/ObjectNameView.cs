using System;
using UnityEngine;

public class ObjectNameView : MonoBehaviour
{
    [SerializeField]
    private string _objectName;
    [SerializeField]
    private int _textSize;
    [SerializeField]
    private Font _textFont;
    [SerializeField]
    private Color _textColor = Color.white;
    [SerializeField]
    private float _textHeight = 0.8f;
    [SerializeField] 

    private bool _isPicked;
    private string _text;
    private GUIStyle style;

    private void Start()
    {
        if (_isPicked)
            enabled = false;
    }
    private void Awake()
    {
        _text = string.Format("<b>Нажмите \"{0}\" чтобы взять </b><color=#ffea00>{1}</color>", "E", _objectName);
        GUI.depth = 9999;
        style = new()
        {
            fontSize = _textSize,
            richText = true,
            alignment = TextAnchor.MiddleCenter
        };
        style.normal.textColor = _textColor;
        if (_textFont) style.font = _textFont;
    }
    void OnGUI()
    {
        Vector3 worldPosition = new(transform.position.x, transform.position.y + _textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), _text, style);
    }
    public void PickUp()
    {
        enabled = false;
        _isPicked = true;
    }


}
