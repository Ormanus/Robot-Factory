using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretBase : Building
{
    public Tilemap tileMap;
    bool _building = false;
    Building p = null;
    private int pylonCount = 0;
    private int robotConstructionLineCount = 0;
    var _AllBuildings; = FindObjectsOfType<Building>()

    public SecretBase(string name) : base(name)
    {
        dimensions = new Vector2(4, 4);
        health = 500;
    }

    public bool CheckMapArea(Vector2 startCoords)
    {
        return true;
    }

    public void CreatePylon(Vector2 startCoords)
    {
        p = new Pylon($"Pylon{pylonCount}");
        pylonCount++;
        _AllBuildings; = FindObjectsOfType<Building>();
        _building = true;

    }

    public void CreateRobotConstructionLine()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        while(_building)
        {
            Vector2 pos = Camera.main.ScreenToWorldPosition(Input.mousePos);
            placeholder.transform.position = pos;
        }
    }
}
