using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortedAnimationUI : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }


    public void PlayAnimation(){
        ResetAnimation();
        animator.speed = 1f;
    }

    public void OnAnimationEnd(){
        ResetAnimation();
        animator.speed = 0f;
    }

    void ResetAnimation(){
        // reset animation to beginning
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash,0,0f);
    }
}
