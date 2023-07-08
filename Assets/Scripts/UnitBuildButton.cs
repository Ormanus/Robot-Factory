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
        metalPrice.text = GetCost("metal").ToString();
        padsPrice.text = GetCost("gamepads").ToString();
        electricityPrice.text = GetCost("electricity").ToString();
        constructionTime.text = unit.constructionTime.ToString();
        icon.sprite = unit.icon;
    }

    public int GetCost(string res)
    {
        for (int i = 0; i < _unit.resourceCosts.Length; i++)
        {
            if (_unit.resourceCosts[i].resource == res)
            {
                return _unit.resourceCosts[i].cost;
            }
        }
        return 0;
    }

    public void Build()
    {
        // TODO: check for a robot factory, add _unit to Q
        if (Selectable.selected.TryGetComponent<RobotConstructionLine>(out var building))
        {
            building.AddRobotToQueue(_unit);
        }
    }
}
