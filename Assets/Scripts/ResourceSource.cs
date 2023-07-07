using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public string resourceName;
    public int amount;

    public int TakeResource(int value)
    {
        if (amount >= value)
        {
            amount -= value;
            return value;
        }
        else
        {
            Destroy(gameObject);
            return amount;
        }
    }
}
