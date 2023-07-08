using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterBot : MonoBehaviour
{
    public GameObject Wings;
    public GameObject BatObject;

    float speed = 2f;
    float chargeDuration = 1f;
    float batDuration = 0.5f;
    float chargeDistance = 2f;

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
    }

    void Move()
    {
        if (hedgehog != null)
        {
            Vector3 hedgeHogPosition = new Vector3(hedgehog.transform.position.x, hedgehog.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, hedgeHogPosition, speed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, hedgeHogPosition - transform.position));

            Wings.transform.localEulerAngles = new Vector3(0, 0, 0);

            if ((hedgeHogPosition - transform.position).magnitude < chargeDistance)
            {
                state = BatterState.Charging;
                timeSinceCharging = 0;
            }
        }
    }

    void Charge()
    {
        timeSinceCharging += Time.deltaTime;

        Vector3 hedgeHogPosition = new Vector3(hedgehog.transform.position.x, hedgehog.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, hedgeHogPosition, speed / 2 * Time.deltaTime);
        transform.rotation = Quaternion.FromToRotation(transform.position, hedgeHogPosition - transform.position);

        Wings.transform.localEulerAngles = new Vector3(0, 0, Wings.transform.localEulerAngles.z + 45 / chargeDuration * Time.deltaTime);

        if (timeSinceCharging > chargeDuration) 
        {
            state = BatterState.Batting;
            timeSinceBatting = 0;
        }
    }

    void Bat()
    {
        Wings.transform.localEulerAngles = new Vector3(0, 0, Wings.transform.localEulerAngles.z - 360 / batDuration * Time.deltaTime);
        timeSinceBatting += Time.deltaTime;
        if (timeSinceBatting > batDuration)
        {
            state = BatterState.Moving;
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
