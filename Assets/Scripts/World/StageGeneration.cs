using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class StageGeneration : MonoBehaviour
{
    [SerializeField]
    private bool _withRepetitions;
    [SerializeField]
    private int _roomsCount;
    [SerializeField]
    private int _roomsDensity;
    [SerializeField]
    private int _bossSpawnDelay;
    [SerializeField]
    private AstarPath _astarPath;
    [SerializeField]
    private GameObject _bossRoom;
    [SerializeField]
    private GameObject _shopRoom;
    [SerializeField]
    private List<GameObject> _roomsPrefabs;
    [SerializeField]
    private GameObject _leftConnection;
    [SerializeField]
    private GameObject _rightConnection;

    private GameObject[,] _roomsMap;
    private int _mapX;
    private int _mapY;
    private GameObject _roomToSpawn;
    private GameObject _lastSpawnedRoom;
    private GameObject _roomToSpawnFrom;
    private List<GameObject> _spawnedRooms;
    private bool _spawnShop;
    private float _delta=0;

    public int BossSpawnDelay { get => _bossSpawnDelay; }

    void Start()
    {
        Initialize();
        GenerateStage();
        _astarPath.Scan();
    }

    private void FixedUpdate()
    {
        if (_delta <= 0)
        {
            _astarPath.Scan();
            _delta = 5f;
        }
        _delta -= Time.fixedDeltaTime;
    }


    private void Initialize()
    {
        _spawnedRooms = new List<GameObject>();
        _roomToSpawn = _roomsPrefabs[0];
        _lastSpawnedRoom = Instantiate(_roomToSpawn, Vector2.zero, Quaternion.identity);
        _lastSpawnedRoom.transform.parent = transform;
        _spawnedRooms.Add(_lastSpawnedRoom);
        _roomsMap = new GameObject[_roomsCount * 2 - 1, _roomsCount * 2 - 1];
        _mapX = _roomsCount - 1;
        _mapY = _roomsCount - 1;
        var room = _lastSpawnedRoom.GetComponent<RoomProperties>();
        room.MapX = _mapX;
        room.MapY = _mapY;
        _roomsMap[_mapX, _mapY] = _lastSpawnedRoom;
        GameController.CurrentRoom = _roomsMap[_mapX, _mapY].GetComponent<RoomProperties>();
    }

    private void GenerateStage()
    {
        for (int i = 1; i < _roomsCount; i++)
        {
            if (i == _roomsCount - 1)
            {
                _roomToSpawnFrom = _spawnedRooms[UnityEngine.Random.Range(1, _spawnedRooms.Count)];
                _spawnShop = true;
            }
            else
                _roomToSpawnFrom = _spawnedRooms[UnityEngine.Random.Range(0, _spawnedRooms.Count)];
            var room = _roomToSpawnFrom.GetComponent<RoomProperties>();
            _mapX = room.MapX;
            _mapY = room.MapY;
            var possibleDirections = GetPossibleDirections();
            if (possibleDirections.Count == 0)
            {
                _spawnedRooms.Remove(_roomToSpawnFrom);
                i--;
                continue;
            }
            var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
            var connectionVector = GetConnectionVector(spawnDirection);
            SelectNextRoom();
            SpawnRoom(connectionVector,spawnDirection);
            _roomsMap[_mapX, _mapY] = _lastSpawnedRoom;
            _lastSpawnedRoom.transform.parent = transform;
            _spawnedRooms.Add(_lastSpawnedRoom);
        }
    }

    private void SelectNextRoom()
    {
        _roomToSpawn = _spawnShop ? _shopRoom : _roomsPrefabs[UnityEngine.Random.Range(1, _roomsPrefabs.Count)];
        if (!_withRepetitions)
            _roomsPrefabs.Remove(_roomToSpawn);
    }

    public void GenerateBossRoom()
    {
        var roomsWithDistance = new Dictionary<GameObject, int>();
        var mapX = GameController.CurrentRoom.MapX;
        var mapY = GameController.CurrentRoom.MapY;

        var maxDistance = 0;
        foreach (var room in _spawnedRooms)
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
            _roomToSpawnFrom = distantRooms[UnityEngine.Random.Range(0, distantRooms.Count)];
            var roomFrom = _roomToSpawnFrom.GetComponent<RoomProperties>();
            _mapX = roomFrom.MapX;
            _mapY = roomFrom.MapY;
            possibleDirections = GetPossibleDirections();
            if (possibleDirections.Count == 0)
            {
                distantRooms.Remove(_roomToSpawnFrom);
                i--;
                continue;
            }
            else break;
        }
        var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];
        var connectionVector = GetConnectionVector(spawnDirection);
        _roomToSpawn = _bossRoom;
        SpawnRoom(connectionVector, spawnDirection);
        _roomsMap[mapX, mapY] = _lastSpawnedRoom;
        _lastSpawnedRoom.transform.parent = transform;
        _spawnedRooms.Add(_lastSpawnedRoom);
    }

    private static Vector2Int GetConnectionVector(Vector2Int spawnDirection)
    {
        return new Vector2Int(Math.Abs(spawnDirection.y) == 1 ? spawnDirection.y : spawnDirection.x,
                                                        Math.Abs(spawnDirection.x) == 1 ? -spawnDirection.x : spawnDirection.y);
    }

    private void SpawnRoom(Vector2Int connectionVector, Vector2Int vector)
    {
        var spawnedRoom = _roomToSpawnFrom.GetComponent<RoomProperties>();
        var maxRoomSize = Math.Max(_roomToSpawn.GetComponent<RoomProperties>().Size,
            spawnedRoom.Size);
        var roomDX = maxRoomSize - _roomsDensity;
        var roomDY = roomDX / 2;
        _lastSpawnedRoom = Instantiate(_roomToSpawn, new Vector3(_roomToSpawnFrom.transform.position.x + roomDX * connectionVector.x,
        _roomToSpawnFrom.transform.position.y + roomDY * connectionVector.y, 0), Quaternion.identity);
        GenerateAiNet();
        var lastRoom = _lastSpawnedRoom.GetComponent<RoomProperties>();
        lastRoom.MapX = spawnedRoom.MapX + vector.x;
        lastRoom.MapY = spawnedRoom.MapY + vector.y;
        _mapX += vector.x;
        _mapY += vector.y;
        MakeConnection(connectionVector, maxRoomSize);
    }

    private void GenerateAiNet()
    {
        var grid = _astarPath.data.AddGraph(typeof(GridGraph)) as GridGraph;
        grid.inspectorGridMode = InspectorGridMode.IsometricGrid;
        grid.isometricAngle = 60;
        grid.collision.use2D = true;
        grid.nodeSize = 0.5f;
        grid.collision.mask = LayerMask.GetMask(new string[] { "column" });
        grid.rotation = new Vector3(45, 270, 270);
        var position = _lastSpawnedRoom.transform.position;
        grid.center = new Vector3(position.x, position.y + 0.244f, position.z);
        if (TryGetComponent<AISize>(out var size))
            grid.SetDimensions(size.Size * (int)Math.Ceiling(1 / grid.nodeSize), size.Size * (int)Math.Ceiling(1 / grid.nodeSize), grid.nodeSize);
    }

    private void MakeConnection(Vector2Int connectionVector, float maxRoomSize)
    {
        var fromRoom = _roomToSpawnFrom.GetComponent<RoomProperties>();
        var minRoomSize = Math.Min(_roomToSpawn.GetComponent<RoomProperties>().Size,
            fromRoom.Size);
        var roomSize = fromRoom.Size;
        var connectionX = _roomToSpawnFrom.transform.position.x + 
                          (roomSize / 2 - (roomSize - 2) / 4) * connectionVector.x;
        var connectionY = _roomToSpawnFrom.transform.position.y + 
                          (roomSize / 2 - (roomSize - 2) / 4) / 2 * connectionVector.y;

        var connectionLength = maxRoomSize + (maxRoomSize - minRoomSize) / 2 - 1 - _roomsDensity * 2;
        for (int connectionsCount = 0; connectionsCount < connectionLength; connectionsCount++)
        {
            var connectionToSpawn = connectionVector.x * connectionVector.y == 1 ? 
                                    _rightConnection :
                                    _leftConnection;
            var connectionPart = Instantiate(connectionToSpawn, new Vector3(connectionX, connectionY, 0), Quaternion.identity);
            connectionPart.transform.parent = transform;
            connectionX += 0.5f * connectionVector.x;
            connectionY += 0.25f * connectionVector.y;
        }
        var lastRoom = _lastSpawnedRoom.GetComponent<RoomProperties>();
        fromRoom.SpawnExit(connectionVector);
        lastRoom.SpawnExit(-connectionVector);
    }

    private List<Vector2Int> GetPossibleDirections()
    {
        var possibleDirections = new List<Vector2Int>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x * y != 0) continue;
                if (_roomsMap[_mapX + x, _mapY + y] == null)
                {
                    possibleDirections.Add(new Vector2Int(x, y));
                }
            }
        return possibleDirections;
    }
}
