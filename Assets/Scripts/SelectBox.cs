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
        Selectable.OnSelect.AddListener(OnSelect);
    }

    void OnSelect(Selectable obj)
    {
        if (obj != null)
        {
            spriteRenderer.size = obj.size * 1.1f;
        }
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
