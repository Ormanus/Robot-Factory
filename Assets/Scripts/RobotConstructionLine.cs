using Outloud.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotConstructionLine : Building
{
    public int maxQueueLength = 3;
    public Image progressCircle;

    private UnitData.Unit? _currentlyBuilding;
    private Queue<UnitData.Unit> _queue = new();
    private void Awake()
    {
        dimensions = new Vector2(2.5f, 2.5f);
        health = 100;
    }

    public bool AddRobotToQueue(UnitData.Unit unit)
    {
        foreach(var neededRes in unit.resourceCosts)
        {
            if(Resources.GetInstance().GetResource(neededRes.resource) < neededRes.cost)
            {
                Debug.Log("Not enough resources to build robot.");
                AudioManager.PlaySound("wrong");
                return false;
            }
        }
        if (_queue.Count < maxQueueLength)
        {
            foreach (var neededRes in unit.resourceCosts)
            {
                Resources.GetInstance().ConsumeResouce(neededRes.resource, neededRes.cost);
            }
            _queue.Enqueue(unit);
            return true;
        }
        Debug.Log("This building's queue is full.");
        AudioManager.PlaySound("wrong");
        return false;
    }

    public IEnumerator BuildRobot()
    {
        _currentlyBuilding = _queue.Dequeue();
        float startTime = Time.time;
        progressCircle.enabled = true;
        while (Time.time < startTime + _currentlyBuilding.Value.constructionTime)
        {
            progressCircle.fillAmount = 1f - (Time.time - startTime) / _currentlyBuilding.Value.constructionTime;
            yield return null;
        }
        progressCircle.enabled = false;
        var obj = Instantiate(_currentlyBuilding.Value.prefab, transform.position, Quaternion.identity);
        _currentlyBuilding = null;
    }

    void Update()
    {
        if (!placed)
            return;

        if (!_currentlyBuilding.HasValue && _queue.Count > 0)
        {
            StartCoroutine(BuildRobot());
        }
    }

    public string GetQueueString()
    {
        return _queue.Count.ToString() + " / " + maxQueueLength.ToString();
    }
}
