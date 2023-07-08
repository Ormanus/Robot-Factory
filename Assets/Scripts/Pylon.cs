using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : Building
{
    const float frequency = 1.0f;
    const int amount = 1;

    float _time;

    public Pylon(string name) : base(name)
    {
        dimensions = new Vector2(2, 2);
        health = 100;
    }

    private void Awake()
    {
        _time = Time.time;
    }

    public static void AddElectricity()
    {
        Resources.GetInstance().GainResouce("electricity", amount); 
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > frequency)
        {
            _time -= frequency;
            AddElectricity();
        }
    }
}
