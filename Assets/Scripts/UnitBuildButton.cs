using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuildButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI metalPrice;
    public TMPro.TextMeshProUGUI padsPrice;
    public TMPro.TextMeshProUGUI electricityPrice;
    public TMPro.TextMeshProUGUI constructionTime;
    public Image icon;

    UnitData.Unit _unit;

    public void Init(UnitData.Unit unit)
    {
        _unit = unit;
        metalPrice.text = unit.metalCost.ToString();
        padsPrice.text = unit.padsCost.ToString();
        electricityPrice.text = unit.electricityCost.ToString();
        constructionTime.text = unit.constructionTime.ToString();
        icon.sprite = unit.icon;
    }

    public void Build()
    {
        // TODO: check for a robot factory, add _unit to Q
        if (Selectable.selected.TryGetComponent<Building>(out var building))
        {
            building.Terminate();
        }
    }
}
