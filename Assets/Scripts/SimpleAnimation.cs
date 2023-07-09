using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class SimpleAnimation : MonoBehaviour
{
    public AnimationController.AnimationList anim;

    private void Start()
    {
        GetComponent<AnimationController>().SetAnimationState(anim);
    }
}
