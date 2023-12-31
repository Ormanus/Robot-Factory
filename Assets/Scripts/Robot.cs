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
}
