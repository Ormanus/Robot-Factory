using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmeraldDisplay : MonoBehaviour
{
    public Image[] emeralds;

    private void Awake()
    {
        Resources.OnEmeraldGained.AddListener(OnEmerald);
        foreach (var emerald in emeralds)
        {
            emerald.enabled = false;
        }
    }

    void OnEmerald(string emeraldName)
    {
        foreach (var emerald in emeralds)
        {
            if (emerald.name == emeraldName)
            {
                emerald.enabled = true;
            }
        }
    }
}
