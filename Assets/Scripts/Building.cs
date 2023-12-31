using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected Vector2 dimensions;
    protected int health;
    public bool placed = false;

    public static List<Building> AllBuildings = new();

    public void Place()
    {
        placed = true;
        AllBuildings.Add(this);
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.position += Vector3.forward;
    }

    private void OnDestroy()
    {
        AllBuildings.Remove(this);
    }

    public int GetHealth()
    {
        return health;
    }

    public void RemoveHealth(int damage)
    {
        this.health -= damage;
    }

    public Vector2 GetDimensions()
    {
        return dimensions;
    }

    //If health reaches zero or lower, return true so that the building can be destroyed.
    public bool Terminate()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
