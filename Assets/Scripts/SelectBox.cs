using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Selectable.OnSelect.AddListener(Select);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Select(Selectable obj)
    {
        if (obj == null) 
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            transform.position = obj.transform.position;
        }
    }
}
