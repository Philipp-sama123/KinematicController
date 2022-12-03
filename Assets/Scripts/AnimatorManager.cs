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
        _playerManager = GetComponent<PlayerManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
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
        if ( _playerManager.isUsingRootMotion == false )
            return;

        float delta = Time.deltaTime;
        Vector3 deltaPosition = animator.deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        _playerLocomotion.rigidbody.drag = 0;

        Debug.LogWarning("[Rigidbody] Applied Velocity on using root Motion:" + velocity);

        _playerLocomotion.rigidbody.velocity = velocity;
    }

}