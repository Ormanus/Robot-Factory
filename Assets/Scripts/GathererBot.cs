using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class GathererBot : MonoBehaviour
{
    int state = 0; // 0 = idle, 1 = move to resource, 2 = gather resource, 3 = return to base
    GameObject _resourceTarget = null;
    AnimationController _anim;
    public float gatherTime = 5f;
    float resourceGatherTimer = 0f;
    
    string _resourceName;
    int _resourceAmount;

    public AnimationController.AnimationList idleAnimation;
    public AnimationController.AnimationList FightAnimation;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
        _anim.SetAnimationState(idleAnimation);
    }

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
                Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var objects = FindObjectsOfType<ResourceSource>();
                foreach (var item in objects)
                {
                    if ((targetPos - (Vector2)item.transform.position).magnitude < 2f)
                    {
                        Debug.Log("Target found!");
                        _resourceTarget = item.gameObject;
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
                Debug.Log("Stopped");
                GetComponent<RobotMovement>().Stop();
                state = 2;
                _anim.SetAnimationState(FightAnimation);
            }
        }
        if (state == 2)
        {
            resourceGatherTimer += Time.deltaTime;
            if (resourceGatherTimer > gatherTime)
            {
                var res = _resourceTarget.GetComponent<ResourceSource>();
                _resourceName = res.resourceName;
                _resourceAmount = res.TakeResource(15);
                resourceGatherTimer = 0f;
                state = 3;
            }
        }
        if (state == 3)
        {
            // TODO: Find closest building, move
            _anim.SetAnimationState(idleAnimation);
            Debug.Log("Resources gathered!");
        }
    }
}
