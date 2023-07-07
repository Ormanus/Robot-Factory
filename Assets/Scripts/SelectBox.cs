using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (Selectable.selected == null)
        {
            spriteRenderer.enabled = false;
            return;
        }

        spriteRenderer.enabled = true;
        transform.position = Selectable.selected.transform.position;
    }
}
