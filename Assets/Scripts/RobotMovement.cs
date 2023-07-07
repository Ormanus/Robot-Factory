using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public GameObject targetAnimation;

    int state = 0;
    Vector2 target;

    private void Awake()
    {
        target = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Selectable.selected == GetComponent<Selectable>())
            {
                state = 1;
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (targetAnimation != null)
                {
                    Instantiate(targetAnimation, target, Quaternion.identity);
                }
            }
        }

        if (state == 1)
        {
            Vector3 delta = ((Vector3)target - transform.position);
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
}
