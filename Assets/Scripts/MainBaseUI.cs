using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBaseUI : MonoBehaviour
{
    public UnitData unitData;
    public GameObject buttonPrefab;
    public Transform buttonRoot;

    List<GameObject> _buttonList = new();

    static MainBaseUI _instance;

    private void Awake()
    {
        _instance = this;
        Selectable.OnSelect.AddListener(OnSlect);
        gameObject.SetActive(false);
    }

    public static void Show()
    {
        _instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        _instance.gameObject.SetActive(false);
    }

    void OnSlect(Selectable obj)
    {
        if (obj != null && obj.GetComponent<MainBase>() != null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

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
