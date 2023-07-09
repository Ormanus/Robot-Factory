using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterBot : MonoBehaviour
{
    public GameObject Wings;
    public GameObject BatObject;

    Collider2D batCollider;
    Collider2D hedgehogCollider;
    Rigidbody2D hedgehogRB;

    RobotMovement robotMovement;

    float speed = 2f;
    float chargeDuration = 1f;
    float batDuration = 0.5f;
    float chargeDistance = 2f;
    float knockback = 12f;

    float timeSinceBatting = 0f;
    float timeSinceCharging = 0f;

    enum BatterState
    {
        Moving,
        Charging,
        Batting,
    }
    BatterState state = BatterState.Moving;

    HedgehogController hedgehog;
    // Start is called before the first frame update
    void Start()
    {
        hedgehog = FindObjectOfType<HedgehogController>();
        robotMovement = GetComponent<RobotMovement>();
        robotMovement.movementSpeed= speed;
        batCollider = BatObject.transform.GetComponent<Collider2D>();
        hedgehogCollider = hedgehog.GetComponent<Collider2D>();
        hedgehogRB = hedgehogCollider.attachedRigidbody;
    }

    void Move()
    {
        if (hedgehog != null)
        {
            robotMovement.SetTarget(hedgehog.transform.position);
            Vector3 hedgeHogPosition = new Vector3(hedgehog.transform.position.x, hedgehog.transform.position.y, transform.position.z);
            //    transform.position = Vector3.MoveTowards(transform.position, hedgeHogPosition, speed * Time.deltaTime);
            //    transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, hedgeHogPosition - transform.position));

            Wings.transform.localEulerAngles = new Vector3(0, 0, 0);

            if ((hedgeHogPosition - transform.position).magnitude < chargeDistance)
            {
                state = BatterState.Charging;
                timeSinceCharging = 0;
                robotMovement.movementSpeed = speed / 2;
            }
            //}
        }
    }
    void Charge()
    {
        robotMovement.SetTarget(hedgehog.transform.position);
        timeSinceCharging += Time.deltaTime;

        //Vector3 hedgeHogPosition = new Vector3(hedgehog.transform.position.x, hedgehog.transform.position.y, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, hedgeHogPosition, speed / 2 * Time.deltaTime);
        //transform.rotation = Quaternion.FromToRotation(transform.position, hedgeHogPosition - transform.position);

        Wings.transform.localEulerAngles = new Vector3(0, 0, Wings.transform.localEulerAngles.z + 45 / chargeDuration * Time.deltaTime);

        if (timeSinceCharging > chargeDuration)
        {
            robotMovement.movementSpeed = 0;
            state = BatterState.Batting;
            timeSinceBatting = 0;
        }
    }

    void Bat()
    {
        Wings.transform.localEulerAngles = new Vector3(0, 0, Wings.transform.localEulerAngles.z - 360 / batDuration * Time.deltaTime);
        timeSinceBatting += Time.deltaTime;

        bool bonked = false;

        if (batCollider.IsTouching(hedgehogCollider))
        {
            float angle = BatObject.transform.rotation.eulerAngles.z / 180 * Mathf.PI;

            hedgehogRB.velocity = new Vector2(Mathf.Cos(angle + Mathf.PI / 2), Mathf.Sin(angle + Mathf.PI / 2)) * knockback;
            bonked = true;
            hedgehog.DropEmeralds();
        }

        if (timeSinceBatting > batDuration || bonked)
        {
            state = BatterState.Moving;
            robotMovement.movementSpeed = speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BatterState.Moving:
                Move();
                break;
            case BatterState.Charging:
                Charge();
                break;
            case BatterState.Batting:
                Bat();
                break;
        }
    }
}
