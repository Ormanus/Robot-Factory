using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgehogController : MonoBehaviour
{
    AnimationController controller;

    public AnimationController.AnimationList animationIdle;
    public AnimationController.AnimationList animationRun;
    public AnimationController.AnimationList animationAttack;

    enum HedgehogState
    {
        Idle,
        Run,
        Charge,
        Attack
    }
    HedgehogState hedgehogState;

    List<GameObject> emeralds = new();

    void Start()
    {
        controller = GetComponent<AnimationController>();

        SetState(HedgehogState.Idle);
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

    void Idle()
    {

    }

    void Run()
    {

    }

    void Attack()
    {

    }

    void Update()
    {
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
