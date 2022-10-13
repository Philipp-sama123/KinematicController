using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// calls and runs everything 
//runs all functionality for the player
public class PlayerManager : MonoBehaviour {

    private InputManager inputManager;
    private CameraManager cameraManager;
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;

    public bool isInteracting = false;
    public bool isUsingRootMotion = false;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        // playerLocomotion.HandleRollingAndSprinting(deltaTime); -- maybe root motion movememnt here 
        // playerLocomotion.HandleJumping(); // playerLocomotion.HandleJumping();

    }

    // When you do stuff with a Rigidbody - everything runs much smoother and nicer with fixed Update 
    // Because it gets called each frame per second (?) 
    // - [Unity Specific rule]
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;
        
        if ( cameraManager != null )
            cameraManager.HandleAllCameraMovement(deltaTime, inputManager.cameraInputX, inputManager.cameraInputY);
        else Debug.LogWarning("[Error] No Camera found!");

        isInteracting = animatorManager.animator.GetBool("IsInteracting");
        isUsingRootMotion = animatorManager.animator.GetBool("IsUsingRootMotion");
        playerLocomotion.isJumping = animatorManager.animator.GetBool("IsJumping"); // disable Jumping when it was played - on the animation

        animatorManager.animator.SetBool("IsGrounded", playerLocomotion.isGrounded); // Animation transition
    }
}