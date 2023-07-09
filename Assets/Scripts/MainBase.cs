using Outloud.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Building
{
    Building _buildingPlaceholder = null;
    private void Awake()
    {
        dimensions = new Vector2 (3, 3);
    }


    public void PlacementMode(UnitData.Unit building)
    {
        MainBaseUI.Hide();
        _buildingPlaceholder = Instantiate(building.prefab).GetComponent<Building>();
    }

    private void Update()
    {
        if (_buildingPlaceholder != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(_buildingPlaceholder.gameObject);
                _buildingPlaceholder = null;
                MainBaseUI.Show();
                return;
            }

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _buildingPlaceholder.transform.position = pos;

            // Check if building is allowed at this position
            bool allowed = CheckCollisions();
            _buildingPlaceholder.GetComponent<SpriteRenderer>().color = allowed ? Color.green : Color.red;

            if (Input.GetMouseButtonDown(0))
            {
                if (allowed)
                {
                    AudioManager.PlaySound("newBuilding");
                    _buildingPlaceholder.Place();
                    _buildingPlaceholder = null;
                    MainBaseUI.Show();
                }
                else
                {
                    AudioManager.PlaySound("wrong");
                }
            }
        }
    }

    bool CheckCollisions()
    {
        Vector2 pos = _buildingPlaceholder.transform.position;
        Bounds a = new Bounds(pos, _buildingPlaceholder.GetDimensions());
        foreach (var building in AllBuildings)
        {
            Bounds b = new Bounds(building.transform.position, building.GetDimensions());

            if (a.Intersects(b))
            {
                return false;
            }
        }

        float w = a.extents.x; 
        float h = a.extents.y;

        Vector2[] corners = new Vector2[]
        {
            a.center + new Vector3(w, h),
            a.center + new Vector3(-w, h),
            a.center + new Vector3(-w, -h),
            a.center + new Vector3(w, -h),
        };

        foreach (var corner in corners)
        {
            if (WaterColliders.IsInWater(corner))
                return false;
        }
        return true;
    }

    public static bool IsBuilding()
    {
        return FindObjectOfType<MainBase>()._buildingPlaceholder != null;
    }
}
