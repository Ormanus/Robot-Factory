using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public GameObject targetAnimation;
    public AnimationController.AnimationList walkAnimation;

    int state = 0;
    Vector2 _target;
    AnimationController _anim;

    public UnityEvent OnDestinationReached = new();

    private void Awake()
    {
        _target = transform.position;
        _anim = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Selectable.selected == GetComponent<Selectable>())
            {
                state = 1;
                _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (_anim != null ) 
                    _anim.SetAnimationState(walkAnimation);

                if (targetAnimation != null)
                {
                    Instantiate(targetAnimation, _target, Quaternion.identity);
                }
            }
        }

        if (state == 1)
        {
            Vector3 delta = ((Vector3)_target - transform.position);
            if (delta.magnitude > 0.5f)
            {
                transform.position += delta.normalized * movementSpeed * Time.deltaTime;
                transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, delta));

                if (WaterColliders.IsInWater(transform.position))
                {
                    ExplosionFactory.CreateBoom(transform.position, 2f);
                    Destroy(gameObject);
                }
            }
            else
            {
                Stop();
                OnDestinationReached?.Invoke();
            }
        }
    }

    public void SetTarget(Vector2 target)
    {
        if (_anim!= null)
            _anim.SetAnimationState(walkAnimation);

        _target = target;
        state = 1;
    }

    public void Stop()
    {
        if (_anim != null)
            _anim.Stop();

        state = 0;
    }
}
