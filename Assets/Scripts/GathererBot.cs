using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RobotMovement))]
public class GathererBot : MonoBehaviour
{
    public float gatherTime = 5f;
    public Transform emeraldCarrier;

    int state = 0; // 0 = idle, 1 = move to resource, 2 = gather resource, 3 = return to base
    GameObject _resourceTarget = null;
    AnimationController _anim;
    RobotMovement _movement;
    float _resourceGatherTimer = 0f;

    string _resourceName;
    int _resourceAmount;

    GameObject _emerald;

    GameObject _homeTarget = null;

    public AnimationController.AnimationList idleAnimation;
    public AnimationController.AnimationList FightAnimation;

    public static UnityEvent OnResourceGathered = new();

    private void Awake()
    {
        _movement = GetComponent<RobotMovement>();
        _anim = GetComponent<AnimationController>();
        _movement.OnDestinationReached.AddListener(DestinationReached);
    }

    private void Start()
    {
        _anim.SetAnimationState(idleAnimation);
    }

    public void SetResourceTarget(GameObject resourceTarget)
    {
        _resourceTarget = resourceTarget;
    }

    private void OnDestroy()
    {
        if (_emerald != null)
        {
            _emerald.GetComponent<Selectable>().enabled = true;
        }
    }

    void DestinationReached()
    {
        if (state == 3)
        {
            FindHome();
            if (_homeTarget != null)
            {
                _movement.SetTarget(_homeTarget.transform.position);
            }
        }
        if (state == 1)
        {
            if (_resourceTarget != null)
                _movement.SetTarget(_resourceTarget.transform.position);
        }
    }

    private void Update()
    {
        if (_emerald != null)
        {
            _emerald.transform.position = emeraldCarrier.position;
        }

        if (Input.GetMouseButtonDown(1) && state != 3) 
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
            if (_resourceTarget == null || _resourceTarget.IsDestroyed())
            {
                state = 0;
                return;
            }
            Vector3 delta = (_resourceTarget.transform.position - transform.position);
            if (delta.magnitude < 1.2f)
            {
                Debug.Log("Stopped");
                _movement.Stop();
                state = 2;
                _anim.SetAnimationState(FightAnimation);
            }
        }
        if (state == 2)
        {
            if (_resourceTarget == null)
            {
                state = 0;
                return;
            }
            Vector3 delta = (_resourceTarget.transform.position - transform.position);
            if (delta.magnitude > 1.2f)
            {
                state = 0;
                return;
            }
            _resourceGatherTimer += Time.deltaTime;
            if (_resourceGatherTimer > gatherTime)
            {
                var res = _resourceTarget.GetComponent<ResourceSource>();
                _resourceName = res.resourceName;

                OnResourceGathered?.Invoke();

                if (_resourceName.Contains("emerald_"))
                {
                    _emerald = _resourceTarget;
                    _emerald.GetComponent<Selectable>().enabled = false;
                }
                else
                {
                    _resourceAmount = res.TakeResource(15);
                }

                _resourceGatherTimer = 0f;
                FindHome();
                state = 3;
                _movement.SetTarget(_homeTarget.transform.position);
            }
        }
        if (state == 3)
        {
            if (_homeTarget != null && _homeTarget.IsDestroyed())
            {
                _homeTarget = null;
            }
            if (_homeTarget == null)
            {
                FindHome();
                if (_homeTarget != null)
                {
                    _movement.SetTarget(_homeTarget.transform.position);
                }
                else
                {
                    _anim.SetAnimationState(idleAnimation);
                    return;
                }
            }

            Vector2 delta = (_homeTarget.transform.position - transform.position);
            if (delta.magnitude < 3f)
            {
                Debug.Log("Home reached!");
                if (_emerald != null)
                {
                    Destroy(_emerald);
                }
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
