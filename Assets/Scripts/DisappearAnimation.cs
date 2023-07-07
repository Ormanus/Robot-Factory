using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAnimation : MonoBehaviour
{
    public float time = 0.5f;
    float startTime = 0;

    private void Awake()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float t = Time.time - startTime;
        t /= time;

        transform.localScale = Vector3.one * (1f - t);

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
