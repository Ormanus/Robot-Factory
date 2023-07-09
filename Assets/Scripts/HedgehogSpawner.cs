using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogSpawner : MonoBehaviour
{
    public GameObject hedgehog;
    private void OnDestroy()
    {
        GathererBot.OnResourceGathered.RemoveListener(SpawnHedgehog);
    }
    // Start is called before the first frame update
    void Start()
    {
        GathererBot.OnResourceGathered.AddListener(SpawnHedgehog);
    }

    void SpawnHedgehog()
    {
        if (FindObjectOfType<HedgehogController>() == null)
            Instantiate(hedgehog, transform.position, Quaternion.identity);
    }
}
