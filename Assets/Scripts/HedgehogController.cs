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

    // Start is called before the first frame update
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

    // Update is called once per frame
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
}
