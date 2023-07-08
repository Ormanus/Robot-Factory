using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColliders : MonoBehaviour
{
    public Collider2D[] colliders;
    static WaterColliders _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static bool IsInWater(Vector2 pos)
    {
        foreach (var collider in _instance.colliders)
        {
            if (collider.OverlapPoint(pos))
                return true;
        }
        return false;
    }
}
