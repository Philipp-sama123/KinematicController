using UnityEngine;

// calls and runs everything 
//runs all functionality for the player
public class PlayerManager : MonoBehaviour {

    private InputManager inputManager;
    private CameraManager cameraManager;
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;

    public bool isUsingRootMotion = false;

    [Header("Player Flags")]
    public bool isInteracting = false;
    public bool isJumping = false;
    public bool isSprinting;
    public bool isGrounded; 
    public bool isInAir;
    // public bool canDoCombo;
    // public bool isUsingRightHand;
    // public bool isUsingLeftHand;
    // public bool isInvulnerable;

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
        animatorManager.UpdateAnimatorValues(0, inputManager.moveAmount, isSprinting);
        if ( inputManager.jumpInput )
        {
            inputManager.jumpInput = false;
            playerLocomotion.HandleJumping();
        }
        if ( inputManager.dodgeInput == true )
        {
            inputManager.dodgeInput = false;
            // todo make as in dark souls game inputHandler.rollFlag = false;
            // todo make dodging flag -> then handle in Player Manager

            playerLocomotion.HandleDodge();
        }
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
        isJumping = animatorManager.animator.GetBool("IsJumping"); // disable Jumping when it was played - on the animation

        animatorManager.animator.SetBool("IsGrounded", isGrounded); // Animation transition
    }
}