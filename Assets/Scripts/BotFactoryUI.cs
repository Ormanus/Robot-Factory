using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFactoryUI : MonoBehaviour
{
    public UnitData unitData;
    public GameObject buttonPrefab;
    public Transform buttonRoot;

    List<GameObject> _buttonList = new();

    // TODO: enable when robot factory is selected

    private void OnEnable()
    {
        foreach (var unit in unitData.units)
        {
            var obj = Instantiate(buttonPrefab, buttonRoot);
            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            obj.GetComponent<UnitBuildButton>().Init(unit);
            _buttonList.Add(obj);
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttonList)
        {
            Destroy(button);
        }
    }
}
