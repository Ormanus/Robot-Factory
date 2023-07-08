using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Robot : ScriptableObject
{
    string robotName;
    int health;
    public Robot(string name)
    {
        robotName = name;
    }

    public abstract void Attack();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
