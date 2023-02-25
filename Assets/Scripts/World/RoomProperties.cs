using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    [SerializeField]
    private float _size;
    [SerializeField]
    private List<Exit> _exits;
    [SerializeField]
    private List<Exit> _walls;

    private bool _isCleared;
    private List<GameObject> _spawnedExits = new();
    private List<GameObject> _enemies = new();
    private int _mapX;
    private int _mapY;

    public int MapX { get => _mapX; set => _mapX = value; }
    public int MapY { get => _mapY; set => _mapY = value; }
    public float Size { get => _size;}
    public bool IsCleared { get => _isCleared;}

    [Serializable]
    public class Exit
    {
        public Vector2Int Cord;
        public GameObject Object;
    }

    void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.CompareTag("Enemy"))
                _enemies.Add(t.gameObject);
        }
        if (_enemies.Count == 0)
            _isCleared = true;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
            OpenExits();
    }

    public void SpawnExit(Vector2Int dir)
    {

        var exit = _exits.Where(x => x.Cord == dir).FirstOrDefault();
        var wall = _walls.Where(x => x.Cord == dir).FirstOrDefault();
        if (wall!=null&&wall.Object)
            wall.Object.SetActive(false);
        exit.Object.SetActive(true);
        exit.Object.GetComponent<EdgeCollider2D>().isTrigger = true;
        _spawnedExits.Add(exit.Object);
    }

    public void OpenExits()
    {
        foreach (var exit in _spawnedExits)
        {
            foreach (var gate in exit.GetComponentsInChildren<Animator>())
            {
                gate.SetBool("Open", true);
                gate.SetBool("Close", false);
            }

            exit.GetComponent<EdgeCollider2D>().isTrigger = true;
        }
        _isCleared = true;
        GameController.IncreaseClearedRoomsCount();
    }

    public void CloseExits()
    {
        if (_isCleared) return;

        foreach (var exit in _spawnedExits)
        {
            foreach (var gate in exit.GetComponentsInChildren<Animator>())
            {
                gate.SetBool("Close", true);
                gate.SetBool("Open", false);
            }

            exit.GetComponent<EdgeCollider2D>().isTrigger = false;
        }

        foreach (var enemy in _enemies)
        {
            if (TryGetComponent<ShootSystem>(out var shootSystem))
                shootSystem.SetTargetOnPlayer();
        }
    }
}
