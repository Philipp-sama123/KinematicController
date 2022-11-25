using System;
using UnityEngine;

public class AnimatorManager : MonoBehaviour {
    public Animator animator;
    private PlayerManager _playerManager;
    private PlayerLocomotion _playerLocomotion;

    public int isUsingRootMotion { get; } = Animator.StringToHash("IsUsingRootMotion");
    private int vertical { get; } = Animator.StringToHash("Vertical");
    private int horizontal { get; } = Animator.StringToHash("Horizontal");

    private void Awake()
    {
        animator = GetComponent<Animator>();

        _playerManager = GetComponentInParent<PlayerManager>();
        _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        animator.SetFloat(horizontal, isSprinting ? horizontalMovement * 2 : horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, isSprinting ? verticalMovement * 2 : verticalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool useRootMotion = false)
    {
        animator.SetBool("IsUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    private void OnAnimatorMove()
    {
        if ( _playerManager.isUsingRootMotion )
        {
            Debug.Log("Animator Move: " + animator.pivotPosition);
            _playerLocomotion.playerRigidbody.MovePosition(transform.position + animator.pivotPosition* 100);
        }
    }
}