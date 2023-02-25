using UnityEngine;

public class AISize : MonoBehaviour
{
    [SerializeField]
    private int _size;

    public int Size { get => _size; set => _size = value; }
}
