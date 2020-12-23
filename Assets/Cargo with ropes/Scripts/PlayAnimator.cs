using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimator : MonoBehaviour
{
    public Animator Animator;
    public AnimationClip animation;
    public AnimationClip animationReversed;

    public void PlayAnimation(bool play)
    {
        if (Animator == null) Animator = GetComponent<Animator>();
        if (play) Animator.Play(animation.name);
        else Animator.Play(animationReversed.name);
    }
}
