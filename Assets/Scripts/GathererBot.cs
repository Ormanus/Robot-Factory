using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class GathererBot : MonoBehaviour
{
    int state = 0; // 0 = idle, 1 = move to resource, 2 = gather resource, 3 = return to base
    GameObject _resourceTarget = null;
    public float gatherTime = 5f;
    float resourceGatherTimer = 0f;

    public void SetResourceTarget(GameObject resourceTarget)
    {
        _resourceTarget = resourceTarget;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            if (Selectable.selected == GetComponent<Selectable>())
            {
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var objects = FindObjectsOfType<GameObject>();
                foreach (var item in objects)
                {
                    if (item.name == "resource" && (targetPos - item.transform.position).magnitude < 1f)
                    {
                        _resourceTarget = item;
                        GetComponent<RobotMovement>().SetTarget(_resourceTarget.transform.position);
                        state = 1;
                        break;
                    }
                }
            }
        }

        if (state == 1)
        {
            Vector3 delta = (_resourceTarget.transform.position - transform.position);
            if (delta.magnitude < 2)
            {
                state = 2;
            }
        }
        if (state == 2)
        {
            resourceGatherTimer += Time.deltaTime;
            if (resourceGatherTimer > gatherTime)
            {
                resourceGatherTimer = 0f;
                state = 3;
            }
        }
        if (state == 3)
        {
            // TODO: Find closest building, move
            Debug.Log("Resources gathered!");
        }
    }
}
