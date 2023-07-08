using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public string description;
    public Vector2 size;

    public static Selectable selected = null;
    public static UnityEvent<Selectable> OnSelect = new();

    bool clicking = false;
    private void OnMouseDown()
    {
        clicking = true;
    }

    private void OnMouseUp()
    {
        if (clicking)
        {
            if (selected != this)
            {
                selected = this;
            }
            else
            {
                selected = null;
            }
            OnSelect?.Invoke(selected);
        }
    }

    private void OnMouseExit()
    {
        clicking = false;
    }
}
