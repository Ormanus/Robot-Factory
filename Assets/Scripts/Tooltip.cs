using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        Selectable.OnSelect.AddListener(OnSelect);
        text.text = "";
    }

    void OnSelect(Selectable obj)
    {
        if (obj != null)
        {
            text.text = obj.description;
        }
        else
        {
            text.text = "";
        }
    }
}
