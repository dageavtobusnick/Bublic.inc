using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class StageGeneration : MonoBehaviour
{
    public bool WithRepetitions;
    public int RoomsCount;
    public int RoomsDensity;
    public int BossSpawnDelay;
    public AstarPath AstarPath;
    public GameObject BossRoom;
    public GameObject ShopRoom;
    public List<GameObject> RoomsPrefabs;
    public GameObject LeftConnection;
    public GameObject RightConnection;

    private GameObject[,] RoomsMap;
    private int _mapX;
    private int _mapY;

    private GameObject roomToSpawn;
    private GameObject lastSpawnedRoom;
    private GameObject roomToSpawnFrom;

    private List<GameObject> spawnedRooms;

    private bool spawnShop;
    private float delta=0;

    void Start()
    {
        Initialize();
        
        GenerateStage();

        AstarPath.Scan();
    }

    private void Update()
    {
        if (delta <= 0)
        {
            AstarPath.Scan();
            delta = 3f;
        }
        delta -= Time.deltaTime;
    }


    private void Initialize()
    {
        spawnedRooms = new List<GameObject>();
        roomToSpawn = RoomsPrefabs[0];
        lastSpawnedRoom = Instantiate(roomToSpawn, Vector2.zero, Quaternion.identity);
        lastSpawnedRoom.transform.parent = transform;
        spawnedRooms.Add(lastSpawnedRoom);
        RoomsMap = new GameObject[RoomsCount * 2 - 1, RoomsCount * 2 - 1];
        _mapX = RoomsCount - 1;
        _mapY = RoomsCount - 1;
        lastSpawnedRoom.GetComponent<RoomProperties>().MapX = _mapX;
        lastSpawnedRoom.GetComponent<RoomProperties>().MapY = _mapY;
        RoomsMap[_mapX, _mapY] = lastSpawnedRoom;
        GameController.CurrentRoom = RoomsMap[_mapX, _mapY].GetComponent<RoomProperties>();
    }

    private void GenerateStage()
    {
        for (int i = 1; i < RoomsCount; i++)
        {
            if (i == RoomsCount - 1)
            {
                roomToSpawnFrom = spawnedRooms[UnityEngine.Random.Range(1, spawnedRooms.Count)];
                spawnShop = true;
            }
            else
                roomToSpawnFrom = spawnedRooms[UnityEngine.Random.Range(0, spawnedRooms.Count)];
            var room = roomToSpawnFrom.GetComponent<RoomProperties>();
            _mapX = room.MapX;
            _mapY = room.MapY;
            var possibleDirections = GetPossibleDirections();
            if (possibleDirections.Count == 0)
            {
                spawnedRooms.Remove(roomToSpawnFrom);
                i--;
                continue;
            }
            var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
            var connectionVector = new Vector2Int(Math.Abs(spawnDirection.y) == 1 ? spawnDirection.y : spawnDirection.x,
                                                Math.Abs(spawnDirection.x) == 1 ? -spawnDirection.x : spawnDirection.y);
            SelectNextRoom();
            SpawnRoom(connectionVector,spawnDirection);
            //switch (spawnDirection)
            //{
            //    case Direction.Up:
            //        {
            //            SpawnBossRoom(1, 1, 0, 1);
            //            break;
            //        }
            //    case Direction.Down:
            //        {
            //            SpawnBossRoom(-1, -1, 0, -1);
            //            break;
            //        }
            //    case Direction.Right:
            //        {
            //            SpawnBossRoom(1, -1, 1, 0);
            //            break;
            //        }
            //    case Direction.Left:
            //        {
            //            SpawnBossRoom(-1, 1, -1, 0);
            //            break;
            //        }
            //    default:
            //        break;
            //}
            RoomsMap[_mapX, _mapY] = lastSpawnedRoom;
            lastSpawnedRoom.transform.parent = transform;
            spawnedRooms.Add(lastSpawnedRoom);
        }
    }

    private void SelectNextRoom()
    {
        roomToSpawn = spawnShop ? ShopRoom : RoomsPrefabs[UnityEngine.Random.Range(1, RoomsPrefabs.Count)];
        if (!WithRepetitions)
            RoomsPrefabs.Remove(roomToSpawn);
    }

    public void GenerateBossRoom()
    {
        var roomsWithDistance = new Dictionary<GameObject, int>();
        var mapX = GameController.CurrentRoom.MapX;
        var mapY = GameController.CurrentRoom.MapY;

        var maxDistance = 0;
        foreach (var room in spawnedRooms)
        {
            var distance = 0;
            var roomData = room.GetComponent<RoomProperties>();
            distance += Math.Abs(roomData.MapX - mapX);
            distance += Math.Abs(roomData.MapY - mapY);
            roomsWithDistance.Add(room, distance);

            if (distance > maxDistance)
                maxDistance = distance;
        }

        var distantRooms = new List<GameObject>();
        foreach (var room in roomsWithDistance)
            if (room.Value == maxDistance)
                distantRooms.Add(room.Key);

        var possibleDirections = new List<Vector2Int>();
        for (int i = 0; i < distantRooms.Count; i++)
        {
            roomToSpawnFrom = distantRooms[UnityEngine.Random.Range(0, distantRooms.Count)];
            var roomFrom = roomToSpawnFrom.GetComponent<RoomProperties>();
            _mapX = roomFrom.MapX;
            _mapY = roomFrom.MapY;
            possibleDirections = GetPossibleDirections();

            if (possibleDirections.Count == 0)
            {
                distantRooms.Remove(roomToSpawnFrom);
                i--;
                continue;
            }
            else break;
        }

        var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
        var connectionVector = GetConnectionVector(spawnDirection);
        roomToSpawn = BossRoom;
        SpawnRoom(connectionVector, spawnDirection);
        RoomsMap[mapX, mapY] = lastSpawnedRoom;
        lastSpawnedRoom.transform.parent = transform;
        spawnedRooms.Add(lastSpawnedRoom);
    }

    private static Vector2Int GetConnectionVector(Vector2Int spawnDirection)
    {
        return new Vector2Int(Math.Abs(spawnDirection.y) == 1 ? spawnDirection.y : 0,
                                                        Math.Abs(spawnDirection.x) == 1 ? -spawnDirection.x : 0);
    }

    private void SpawnRoom(Vector2Int connectionVector, Vector2Int vector)
    {
        var maxRoomSize = Math.Max(roomToSpawn.GetComponent<RoomProperties>().Size,
            roomToSpawnFrom.GetComponent<RoomProperties>().Size);
        var roomDX = maxRoomSize - RoomsDensity;
        var roomDY = roomDX / 2;
        lastSpawnedRoom = Instantiate(roomToSpawn, new Vector3(roomToSpawnFrom.transform.position.x + roomDX * connectionVector.x,
        roomToSpawnFrom.transform.position.y + roomDY * connectionVector.y, 0), Quaternion.identity);
        GenerateAiNet();
        var lastRoom = lastSpawnedRoom.GetComponent<RoomProperties>();
        var spawnedRoom = roomToSpawnFrom.GetComponent<RoomProperties>();
        lastRoom.MapX = spawnedRoom.MapX + vector.x;
        lastRoom.MapY = spawnedRoom.MapY + vector.y;
        _mapX += vector.x;
        _mapY += vector.y;
        MakeConnection(connectionVector, maxRoomSize);
    }

    private void GenerateAiNet()
    {
        var grid = AstarPath.data.AddGraph(typeof(GridGraph)) as GridGraph;
        grid.inspectorGridMode = InspectorGridMode.IsometricGrid;
        grid.isometricAngle = 60;
        grid.collision.use2D = true;
        grid.nodeSize = 0.5f;
        grid.collision.mask = LayerMask.GetMask(new string[] { "column" });
        grid.rotation = new Vector3(45, 270, 270);
        var position = lastSpawnedRoom.transform.position;
        grid.center = new Vector3(position.x, position.y + 0.244f, position.z);
        if (TryGetComponent<AISize>(out var size))
            grid.SetDimensions(size.size * (int)Math.Ceiling(1 / grid.nodeSize), size.size * (int)Math.Ceiling(1 / grid.nodeSize), grid.nodeSize);
    }

    private void MakeConnection(Vector2Int connectionVector, float maxRoomSize)
    {
        var minRoomSize = Math.Min(roomToSpawn.GetComponent<RoomProperties>().Size,
            roomToSpawnFrom.GetComponent<RoomProperties>().Size);
        var roomSize = roomToSpawnFrom.GetComponent<RoomProperties>().Size;
        var connectionX = roomToSpawnFrom.transform.position.x + 
                          (roomSize / 2 - (roomSize - 2) / 4) * connectionVector.x;
        var connectionY = roomToSpawnFrom.transform.position.y + 
                          (roomSize / 2 - (roomSize - 2) / 4) / 2 * connectionVector.y;

        var connectionLength = maxRoomSize + (maxRoomSize - minRoomSize) / 2 - 1 - RoomsDensity * 2;
        for (int connectionsCount = 0; connectionsCount < connectionLength; connectionsCount++)
        {
            var connectionToSpawn = connectionVector.x * connectionVector.y == 1 ? 
                                    RightConnection :
                                    LeftConnection;
            var connectionPart = Instantiate(connectionToSpawn, new Vector3(connectionX, connectionY, 0), Quaternion.identity);
            connectionPart.transform.parent = transform;
            connectionX += 0.5f * connectionVector.x;
            connectionY += 0.25f * connectionVector.y;
        }
        var fromRoom = roomToSpawnFrom.GetComponent<RoomProperties>();
        var lastRoom = lastSpawnedRoom.GetComponent<RoomProperties>();
        switch ((connectionVector.x, connectionVector.y))
        {
            case (1, 1):
                fromRoom.SpawnExit(Direction.Up);
                lastRoom.SpawnExit(Direction.Down);
                break;
            case (-1, -1):
                fromRoom.SpawnExit(Direction.Down);
                lastRoom.SpawnExit(Direction.Up);
                break;
            case (1, -1):
                fromRoom.SpawnExit(Direction.Right);
                lastRoom.SpawnExit(Direction.Left);
                break;
            case (-1, 1):
                fromRoom.SpawnExit(Direction.Left);
                lastRoom.SpawnExit(Direction.Right);
                break;
        }
    }

    private List<Vector2Int> GetPossibleDirections()
    {
        var possibleDirections = new List<Vector2Int>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x * y != 0) continue;
                if (RoomsMap[_mapX + x, _mapY + y] == null)
                {
                    possibleDirections.Add(new Vector2Int(x, y));
                }
            }
        return possibleDirections;
    }
}
