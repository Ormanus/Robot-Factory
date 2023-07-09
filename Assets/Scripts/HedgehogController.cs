using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

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
    const float jumpSpeed = 3.0f;
    const float acceleration = 4.0f;

    const float jumpDuration = 1f;
    const float attackDist = 2f;

    const float jumpChargeTime = 0.5f;
    float timeSinceJumpEnd = 0f;

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

    Bounds _hedgehogBounds;
    Bounds _mapBounds;

    float timeSinceJump = 0;

    void Start()
    {
        controller = GetComponent<AnimationController>();
        SetState(HedgehogState.Idle);
        rb = GetComponent<Rigidbody2D>();

        Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        Tilemap tilemap = tilemaps[0];
        foreach (Tilemap map in tilemaps)
        {
            if (map.localBounds.size.x > tilemap.localBounds.size.x || map.localBounds.size.y > tilemap.localBounds.size.y)
            {
                tilemap = map;
            }
        }

        _mapBounds = tilemap.localBounds;

        Collider2D collider2D = rb.GetComponent<Collider2D>();
        _hedgehogBounds = collider2D.bounds;
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
            if (robot.GetComponent<DecoyBot>() != null ) { weight *= BaseWeight / EnemyWeight; }

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

        // Base
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

    void StartJump()
    {
        SetState(HedgehogState.Attack);
        timeSinceJump = 0;
        AudioSource source = GetComponent<AudioSource>();
        if (source != null) { source.Play(); }
    }

    void Idle()
    {
        if (target != null) { SetState(HedgehogState.Run); }
        rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, acceleration * Time.fixedDeltaTime);
    }

    void Run()
    {
        timeSinceJumpEnd += Time.fixedDeltaTime;
        if (target == null) { SetState(HedgehogState.Idle); return; }
        rb.velocity = Vector2.MoveTowards(rb.velocity, (target.transform.position - transform.position).normalized * maxSpeed, acceleration * Time.fixedDeltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, target.transform.position - transform.position))), Time.fixedDeltaTime * 180);

        if (Dist2D(target.transform.position, transform.position) < attackDist && timeSinceJumpEnd > jumpChargeTime)
        {
            StartJump();
        }
        if (WaterColliders.IsInWater(transform.position))
        {
            StartJump();
        }
    }

    private void CheckJumpCollisions()
    {
        Collider2D collider = GetComponent<Collider2D>();
        // Robots
        foreach (RobotMovement robotMovement in FindObjectsByType<RobotMovement>(FindObjectsSortMode.None))
        {
            if (collider.IsTouching(robotMovement.GetComponent<Collider2D>()))
            {
                Debug.Log("Boom");
                ExplosionFactory.CreateBoom(robotMovement.transform.position, 2f);
                Destroy(robotMovement.gameObject);
            }
        }

        // Emeralds
        foreach (ResourceSource emerald in FindObjectsByType<ResourceSource>(FindObjectsSortMode.None))
        {
            if (emerald.resourceName.StartsWith("emerald"))
            {
                if (collider.IsTouching(emerald.GetComponent<Collider2D>()))
                {
                    CollectEmerald(emerald.gameObject);
                }
            }
        }

        // Main Base
        foreach (MainBase mainBase in FindObjectsByType<MainBase>(FindObjectsSortMode.None))
        {
            if (collider.IsTouching(mainBase.GetComponent<Collider2D>()))
            {
                // L + Ratio + Skill issue + RIPBOZO
                SceneManager.LoadScene("Game Over");
            }
        }
    }

    void Attack()
    {
        timeSinceJump += Time.fixedDeltaTime;
        //if (target == null) { SetState(HedgehogState.Idle); return; }
        if (target != null)
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, (target.transform.position - transform.position).normalized * jumpSpeed, acceleration / 2 * Time.fixedDeltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, target.transform.position - transform.position))), Time.fixedDeltaTime * 180);
        }

        CheckJumpCollisions();

        if (timeSinceJump > jumpDuration)
        {
            SetState(HedgehogState.Run);
            timeSinceJumpEnd = 0;
        }
    }

    void FixedUpdate()
    {
        MoveToMapBounds();
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
            SceneManager.LoadScene("Game Over");
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

    void MoveToMapBounds()
    {
        _hedgehogBounds.center = transform.position;
        var minDelta = _hedgehogBounds.min - _mapBounds.min;
        var maxDelta = _hedgehogBounds.max - _mapBounds.max;

        if (minDelta.x < 0)
        {
            transform.position += Vector3.right * -minDelta.x;
        }
        if (minDelta.y < 0)
        {
            transform.position += Vector3.up * -minDelta.y;
        }
        if (maxDelta.x > 0)
        {
            transform.position += Vector3.right * -maxDelta.x;
        }
        if (maxDelta.y > 0)
        {
            transform.position += Vector3.up * -maxDelta.y;
        }
    }
}
