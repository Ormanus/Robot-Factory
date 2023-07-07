using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected string buildingName;
    protected Vector2 dimensions;
    protected int health;

    public int GetHealth()
    {
        return health;
    }

    public void RemoveHealth(int damage)
    {
        this.health -= damage;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
