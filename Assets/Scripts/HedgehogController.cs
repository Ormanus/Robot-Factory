using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HedgehogController : MonoBehaviour
{
    AnimationController controller;

    public AnimationController.AnimationList animationIdle;
    public AnimationController.AnimationList animationRun;
    public AnimationController.AnimationList animationAttack;

    const float BaseWeight = 1.0f;
    const float EmeraldWeight = 1.5f;
    const float EnemyWeight = 2.5f;

    const float maxSpeed = 2.0f;
    const float acceleration = 4.0f;

    Rigidbody2D rb;

    enum HedgehogState
    {
        Idle,
        Run,
        Charge,
        Attack
    }
    HedgehogState hedgehogState;

    List<GameObject> emeralds = new();

    GameObject target;

    void Start()
    {
        controller = GetComponent<AnimationController>();
        SetState(HedgehogState.Idle);
        rb = GetComponent<Rigidbody2D>();
    }

    void SetState(HedgehogState state)
    {
        hedgehogState = state;

        switch (state)
        {
            case HedgehogState.Idle:
                controller.SetAnimationState(animationIdle);
                break;
            case HedgehogState.Run:
                controller.SetAnimationState(animationRun);
                break;
            case HedgehogState.Attack:
                controller.SetAnimationState(animationAttack);
                break;

        }
    }

    float Dist2D(Vector3 a, Vector3 b)
    {
        return (new Vector2(a.x, a.y) - new Vector2(b.x, b.y)).magnitude;
    }

    GameObject LookForTarget()
    {
        bool hasTarget = false;
        float minWeight = 0;
        GameObject newTarget = null;
        // Look for the "nearest" target, certain objects prioritized over others

        foreach (RobotMovement robot in FindObjectsByType<RobotMovement>(FindObjectsSortMode.None))
        {
            float weight = Dist2D(transform.position, robot.transform.position) * EnemyWeight;
            if (!hasTarget || weight < minWeight) 
            {
                hasTarget = true;
                minWeight = weight;
                newTarget = robot.gameObject;
            }
        }

        //Emeralds
        foreach (ResourceSource emerald in FindObjectsByType<ResourceSource>(FindObjectsSortMode.None))
        {
            if (emerald.resourceName.StartsWith("emerald"))
            { 
                float weight = Dist2D(transform.position, emerald.transform.position) * EmeraldWeight;
                if (!hasTarget || weight < minWeight)
                {
                    hasTarget = true;
                    minWeight = weight;
                    newTarget = emerald.gameObject;
                }
            }
        }

        // Base and fake bases
        // TODO: Fake Bases
        foreach (MainBase mainBase in FindObjectsByType<MainBase>(FindObjectsSortMode.None))
        {
            float weight = Dist2D(transform.position, mainBase.transform.position) * BaseWeight;
            if (!hasTarget || weight < minWeight)
            {
                hasTarget = true;
                minWeight = weight;
                newTarget = mainBase.gameObject;
            }
        }

        return newTarget;
    }

    void Idle()
    {
        if (target != null) { SetState(HedgehogState.Run); }
        rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, acceleration * Time.fixedDeltaTime);
    }

    void Run()
    {
        if (target == null) { SetState(HedgehogState.Idle); return; }
        rb.velocity = Vector2.MoveTowards(rb.velocity, (target.transform.position - transform.position).normalized * maxSpeed, acceleration * Time.fixedDeltaTime);
    }

    void Attack()
    {
        if (target == null) { SetState(HedgehogState.Idle); return; }
        rb.velocity = Vector2.MoveTowards(rb.velocity, (target.transform.position - transform.position).normalized * maxSpeed, acceleration / 2 * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        target = LookForTarget();
        switch (hedgehogState)
        {
            case HedgehogState.Idle:
                Idle();
                break;
            case HedgehogState.Run:
                Run();
                break;
            case HedgehogState.Attack:
                Attack();
                break;

        }
    }

    void CollectEmerald(GameObject emeraldObject)
    {
        emeraldObject.SetActive(false);
        emeralds.Add(emeraldObject);

        if (emeralds.Count == 7)
        {
            // TODO: super audio -> end game
        }
    }

    public void DropEmeralds()
    {
        // TODO: play cling cling sound
        while (emeralds.Count > 0)
        {
            emeralds[0].transform.position = transform.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f);
            emeralds[0].SetActive(true);
            emeralds.RemoveAt(0);
        }
    }
}
