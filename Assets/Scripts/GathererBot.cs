using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class GathererBot : MonoBehaviour
{
    public float gatherTime = 5f;

    int state = 0; // 0 = idle, 1 = move to resource, 2 = gather resource, 3 = return to base
    GameObject _resourceTarget = null;
    AnimationController _anim;
    RobotMovement _movement;
    float _resourceGatherTimer = 0f;

    string _resourceName;
    int _resourceAmount;

    GameObject _homeTarget = null;

    public AnimationController.AnimationList idleAnimation;
    public AnimationController.AnimationList FightAnimation;

    private void Awake()
    {
        _movement = GetComponent<RobotMovement>();
        _anim = GetComponent<AnimationController>();
    }

    private void Start()
    {
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
                        _movement.SetTarget(_resourceTarget.transform.position);
                        state = 1;
                        break;
                    }
                }
            }
        }

        if (state == 1)
        {
            if (_resourceTarget == null)
            {
                state = 0;
                return;
            }
            Vector3 delta = (_resourceTarget.transform.position - transform.position);
            if (delta.magnitude < 2)
            {
                Debug.Log("Stopped");
                _movement.Stop();
                state = 2;
                _anim.SetAnimationState(FightAnimation);
            }
        }
        if (state == 2)
        {
            _resourceGatherTimer += Time.deltaTime;
            if (_resourceGatherTimer > gatherTime)
            {
                var res = _resourceTarget.GetComponent<ResourceSource>();
                _resourceName = res.resourceName;
                _resourceAmount = res.TakeResource(15);
                _resourceGatherTimer = 0f;
                state = 3;
            }
        }
        if (state == 3)
        {
            if (_homeTarget == null)
            {
                FindHome();
            };
            if (_homeTarget == null)
            {
                _anim.SetAnimationState(idleAnimation);
                return;
            }
            _movement.SetTarget(_homeTarget.transform.position);

            Vector2 delta = (_homeTarget.transform.position - transform.position);
            if (delta.magnitude < 2f)
            {
                Debug.Log("Home reached!");
                Resources.GetInstance().GainResouce(_resourceName, _resourceAmount);
                _homeTarget = null;
                state = 1;
            }
        }
    }

    void FindHome()
    {
        var buildings = GameObject.FindGameObjectsWithTag("playerBuilding");
        if (buildings.Length == 0)
        {
            // No buildings found, idle.
            _anim.SetAnimationState(idleAnimation);
            return;
        }

        GameObject closest = null;
        float dMin = float.MaxValue;
        for (int i = 0; i < buildings.Length; i++)
        {
            float d = (buildings[i].transform.position - transform.position).magnitude;
            if (d < dMin)
            {
                dMin = d;
                closest = buildings[i];
            }
        }
        if (closest != null)
        {
            _homeTarget = closest;
        }
    }
}
