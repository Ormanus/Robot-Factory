using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFactoryUI : MonoBehaviour
{
    public UnitData unitData;
    public GameObject buttonPrefab;
    public Transform buttonRoot;
    public TMPro.TextMeshProUGUI queueDisplay;

    List<GameObject> _buttonList = new();

    private void Awake()
    {
        Selectable.OnSelect.AddListener(OnSlect);
        gameObject.SetActive(false);
    }

    void OnSlect(Selectable obj)
    {
        if (obj != null && obj.GetComponent<RobotConstructionLine>() != null)
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

    private void FixedUpdate()
    {
        if (Selectable.selected.TryGetComponent<RobotConstructionLine>(out var building))
        {
            queueDisplay.text = "Queue: " + building.GetQueueString();

        }
    }
}
