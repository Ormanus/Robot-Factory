using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotConstructionLine : Building
{
    private Robot currentlyBuilding;
    private Robot[] queue = new Robot[3];
    public UnitData unitData;
    public RobotConstructionLine(string name) : base(name)
    {
        dimensions = new Vector2(2, 3);
        health = 100;
    }
    
    public bool AddRobotToQueue(Robot robotType)
    {
        foreach(var neededRes in robotType._resourcecosts)
        {
            if(Resources.GetInstance().GetResource(neededRes.Key) < neededRes.Value)
            {
                Debug.Log("Not enough resources to build robot.");
                return false;
            }
        }
        for(int i = 0; i < queue.Length; i++)
        {
            if (queue[i] == null)
            {
                queue[i] = robotType;
                return true;
            }
        }
        Debug.Log("This building's queue is full.");
        return false;
    }

    public IEnumerator BuildRobot(UnitData unit)
    {
        currentlyBuilding = queue[0];
        for(int i = 1; i < queue.Length; i++)
            queue[i - 1] = queue[i];

        yield return new WaitForSeconds(5f);
        var obj = Instantiate(unit.prefab, transform.position, Quaternion.identity);
        currentlyBuilding = null;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyBuilding == null && queue[0] != null)
        {
            StartCoroutine(BuildRobot(unitData));
        }
    }
}
