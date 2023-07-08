using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : Building
{
    public Pylon(string name) : base(name)
    {
        dimensions = new Vector2(2, 2);
        health = 100;
    }

    public static void AddElectricity()
    {
        Resources.GetInstance().GainResouce("electricity", 10); 
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
