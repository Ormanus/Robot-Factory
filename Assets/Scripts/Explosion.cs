using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimationController.AnimationList anim;

    private void Start()
    {
        GetComponent<AnimationController>().SetAnimationState(anim);
        Destroy(gameObject, anim.animationDuration * anim.sprites.Length);
    }
}
