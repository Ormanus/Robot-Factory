using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI display;
    public string resourceName;


    private void FixedUpdate()
    {
        display.text = Resources.GetInstance().GetResource(resourceName).ToString();
    }
}
