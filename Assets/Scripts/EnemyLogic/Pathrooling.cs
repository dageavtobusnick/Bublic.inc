using Pathfinding;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pathrooling : MonoBehaviour
{
    [SerializeField]
    private AIDestinationSetter _aIDestinationSetter;
    [SerializeField]
    private Transform[] _patroolPoints;

    public bool CanPathrooling { get => _patroolPoints.Length > 0; }

    public void LoadPoint(GameObject room)
    {
        _patroolPoints = GameObject.FindGameObjectsWithTag("patrol").Where(x => x.transform.parent.gameObject == room).Select(x=>x.transform).ToArray();
    }

    public Transform GetSpecialPoint(Func<Transform,bool> selectFunc,Func<Transform,float> sortFunc)
    {
        return _patroolPoints.Where(selectFunc).OrderBy(sortFunc).First();
    }

    public void Patroling()
    {
        var target = _patroolPoints[Random.Range(0, _patroolPoints.Length - 1)];
        _aIDestinationSetter.target = target;
    }
}
