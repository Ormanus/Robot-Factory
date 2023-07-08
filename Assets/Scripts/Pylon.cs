using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : Building
{
    const float frequency = 1.0f;
    const int amount = 1;

    float _time;

    private void Awake()
    {
        _time = 0f;
        dimensions = new Vector2(1.2f, 2.5f);
        health = 100;
    }

    public static void AddElectricity()
    {
        Resources.GetInstance().GainResouce("electricity", amount); 
    }

    private void Update()
    {
        if (!placed)
            return;

        _time += Time.deltaTime;
        if (_time > frequency)
        {
            _time -= frequency;
            AddElectricity();
        }
    }
}
