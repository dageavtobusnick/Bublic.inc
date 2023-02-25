using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Texture2D _cursor;

    private void Start()
    {
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
