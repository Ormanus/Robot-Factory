using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public GameObject targetAnimation;

    int state = 0;
    Vector2 _target;

    private void Awake()
    {
        _target = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Selectable.selected == GetComponent<Selectable>())
            {
                state = 1;
                _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            }
            else
            {
                state = 0;
            }
        }
    }

    public void SetTarget(Vector2 target)
    {
        _target = target;
        state = 1;
    }

    public void Stop()
    {
        state = 0;
    }
}
