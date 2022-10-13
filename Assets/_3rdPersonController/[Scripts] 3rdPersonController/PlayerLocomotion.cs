using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

    private PlayerManager playerManager;
    private AnimatorManager animatorManager;
    private InputManager inputManager;

    private Vector3 moveDirection;
    private Transform cameraObject;
    public Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    [SerializeField] private float leapingVelocity = 2.5f;
    [SerializeField] private float fallingVelocity = 35f;
    [SerializeField] private float rayCastHeightOffSet = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump Speeds")]
    [SerializeField] private float gravityIntensity = -10f;
    [SerializeField] private float jumpHeight = 2f;

    private void Awake()
    {
        isGrounded = true;
        if ( Camera.main != null ) cameraObject = Camera.main.transform;
        else Debug.LogWarning("[Not Assigned]: Camera");

        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();

        animatorManager = GetComponentInChildren<AnimatorManager>();
    }

    public void HandleAllMovement()
    {
        if ( inputManager != null && playerRigidbody != null )
        {
            HandleFallingAndLanding();

            if ( playerManager.isInteracting ) return;
            if ( isJumping ) return;

            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        // -- What? - allows to move left and right
        // -based on camera Object and horizontalInput
        moveDirection = moveDirection + (cameraObject.right * inputManager.horizontalInput);
        moveDirection.Normalize(); // changes Vector Length to one
        moveDirection.y = 0; // keeps the player on the floor 

        if ( isSprinting ) // because also animations change
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if ( inputManager.moveAmount >= 0.5f )
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if ( targetDirection == Vector3.zero )
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition = transform.position;

        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        if ( !isGrounded && !isJumping )
        {
            if ( !playerManager.isInteracting )
            {
                animatorManager.PlayTargetAnimation("[Airborne] Falling", true);
            }

            animatorManager.animator.SetBool("IsUsingRootMotion", false);

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if ( Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundLayer) )
        {
            if ( !isGrounded && playerManager.isInteracting )
            {
                animatorManager.PlayTargetAnimation("[Airborne] Landing", true);
            }
            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if ( isGrounded && !isJumping )
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
        }
    }

    public void HandleJumping()
    {
        if ( isGrounded )
        {
            animatorManager.animator.SetBool("IsJumping", true);
            animatorManager.PlayTargetAnimation("[Airborne] Jumping Root", true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }
    }


    public void HandleDodge()
        {
            if ( playerManager.isInteracting ) return;

            animatorManager.PlayTargetAnimation("[Common] Dodge", true, true);
            // Todo: Toggle invulnerable State
        }
    }