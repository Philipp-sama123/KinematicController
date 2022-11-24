using System;
using _Scripts__3rdPersonController;
using UnityEngine;

public class AnimatorManager : MonoBehaviour {
    public Animator animator;

    private int Vertical;
    private int Horizontal;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Horizontal = Animator.StringToHash("Horizontal");
        Vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        animator.SetFloat(Horizontal, isSprinting ? horizontalMovement * 2 : horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(Vertical, isSprinting ? verticalMovement * 2 : verticalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("IsInteracting", isInteracting);
        animator.SetBool("IsUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    private void OnAnimatorMove()
    {
        Debug.Log("Animator Move");
    }
}