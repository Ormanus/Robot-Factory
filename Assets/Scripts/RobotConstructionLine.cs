using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotConstructionLine : Building
{
    public int maxQueueLength = 3;

    private UnitData.Unit? _currentlyBuilding;
    private Queue<UnitData.Unit> _queue = new();
    public RobotConstructionLine(string name) : base(name)
    {
        dimensions = new Vector2(2, 3);
        health = 100;
    }
    
    public bool AddRobotToQueue(UnitData.Unit unit)
    {
        foreach(var neededRes in unit.resourceCosts)
        {
            if(Resources.GetInstance().GetResource(neededRes.Item1) < neededRes.Item2)
            {
                Debug.Log("Not enough resources to build robot.");
                return false;
            }
        }
        if (_queue.Count < maxQueueLength)
        {
            _queue.Enqueue(unit);
            return true;
        }
        Debug.Log("This building's queue is full.");
        return false;
    }

    public IEnumerator BuildRobot()
    {
        _currentlyBuilding = _queue.Dequeue();
        yield return new WaitForSeconds(_currentlyBuilding.Value.constructionTime);
        var obj = Instantiate(_currentlyBuilding.Value.prefab, transform.position, Quaternion.identity);   
    }

    void Update()
    {
        if (_currentlyBuilding.HasValue && _queue.Count > 0)
        {
            StartCoroutine(BuildRobot());
        }
    }
}
