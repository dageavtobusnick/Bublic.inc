using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    public float Size;
    public int MapX;
    public int MapY;

    public GameObject TopExit;
    public GameObject BottomExit;
    public GameObject RightExit;
    public GameObject LeftExit;

    public GameObject TopWall;
    public GameObject BottomWall;
    public GameObject RightWall;
    public GameObject LeftWall;

    public bool IsCleared;

    public List<GameObject> SpawnedExits = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.CompareTag("Enemy"))
                Enemies.Add(t.gameObject);
        }

        if (Enemies.Count == 0)
            IsCleared = true;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        Enemies.Remove(enemy);

        if (Enemies.Count == 0)
            OpenExits();
    }

    public void SpawnExit(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                if (TopWall)
                    TopWall.SetActive(false);
                TopExit.SetActive(true);
                TopExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(TopExit);
                break;
            case Direction.Down:
                if (BottomWall)
                    BottomWall.SetActive(false);
                BottomExit.SetActive(true);
                BottomExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(BottomExit);
                break;
            case Direction.Right:
                if (RightWall)
                    RightWall.SetActive(false);
                RightExit.SetActive(true);
                RightExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(RightExit);
                break;
            case Direction.Left:
                if (LeftWall)
                    LeftWall.SetActive(false);
                LeftExit.SetActive(true);
                LeftExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(LeftExit);
                break;
        }
    }

    public void OpenExits()
    {
        foreach (var exit in SpawnedExits)
        {
            foreach (var gate in exit.GetComponentsInChildren<Animator>())
            {
                gate.SetBool("Open", true);
                gate.SetBool("Close", false);
            }

            exit.GetComponent<EdgeCollider2D>().isTrigger = true;
        }

        IsCleared = true;
        GameController.IncreaseClearedRoomsCount();
    }

    public void CloseExits()
    {
        if (IsCleared) return;

        foreach (var exit in SpawnedExits)
        {
            foreach (var gate in exit.GetComponentsInChildren<Animator>())
            {
                gate.SetBool("Close", true);
                gate.SetBool("Open", false);
            }

            exit.GetComponent<EdgeCollider2D>().isTrigger = false;
        }

        foreach (var enemy in Enemies)
        {
            if (TryGetComponent<ShootSystem>(out var shootSystem))
                shootSystem.SetTargetOnPlayer();
        }
    }
}
