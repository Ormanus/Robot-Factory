using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [System.Serializable]
    public class AnimationList
    {
        public Sprite[] sprites;
        public float animationDuration = 0.2f;
    }

    SpriteRenderer sr;

    int spriteIndex = 0;
    int nSprites = 0;
    float timeSinceLastChange = 0;
    AnimationList animationState;

    public void SetAnimationState(AnimationList state)
    {
        if (animationState == state) { return; }
        animationState = state;

        spriteIndex = 0;
        nSprites = state.sprites.Length;
        sr.sprite = animationState.sprites[0];
    }

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animationState == null) { return; }

        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange > animationState.animationDuration)
        {
            int addition = Mathf.FloorToInt(timeSinceLastChange / animationState.animationDuration);
            timeSinceLastChange -= animationState.animationDuration * addition;
            spriteIndex = (spriteIndex + addition) % nSprites;
            sr.sprite = animationState.sprites[spriteIndex];
        }
    }
}
