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
    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Speeds")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump Speeds")]
    [SerializeField] private float gravityIntensity = -10f;
    [SerializeField] private float jumpHeight = 3f;

    private void Awake()
    {
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
            HandleFalling();

            if ( playerManager.isInteracting ) return;
            if ( playerManager.isJumping ) return;

            HandleMovement();
            HandleRotation();
        }
        else Debug.LogWarning("[Not Assigned]: inputManager or playerRigidbody ");
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        // -- What? - allows to move left and right
        // -based on camera Object and horizontalInput
        moveDirection += (cameraObject.right * inputManager.horizontalInput);
        moveDirection.Normalize(); // changes Vector Length to one
        moveDirection.y = 0; // keeps the player on the floor 

        if ( playerManager.isSprinting ) // because also animations change
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            if ( inputManager.moveAmount >= 0.5f )
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if ( targetDirection == Vector3.zero )
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFalling()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition = transform.position;

        rayCastOrigin.y += rayCastHeightOffSet;

        if ( !playerManager.isGrounded && !playerManager.isJumping )
        {
            if ( !playerManager.isInteracting )
            {
                animatorManager.PlayTargetAnimation("[Airborne] Falling", true);
            }

            animatorManager.animator.SetBool("IsUsingRootMotion", false);

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        Debug.DrawRay(rayCastOrigin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red);
        if ( Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, minimumDistanceNeededToBeginFall, groundLayer) )
        {
            if ( !playerManager.isGrounded && playerManager.isInteracting )
            {
                if ( inAirTimer > 0.25f )
                {
                    animatorManager.PlayTargetAnimation("[Airborne] Landing", true);
                }
                else
                {
                    animatorManager.PlayTargetAnimation("Empty", false);
                }

            }
            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;

            inAirTimer = 0;
            playerManager.isGrounded = true;
        }
        else
        {
            playerManager.isGrounded = false;

        }

        if ( playerManager.isGrounded && !playerManager.isJumping )
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.2f);
        }
    }

    public void HandleJumping()
    {
        if ( playerManager.isGrounded )
        {
            animatorManager.animator.SetBool("IsJumping", true);
            animatorManager.PlayTargetAnimation("[Airborne] Jumping Root", true);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }

        // Todo: Running Jump
        // Todo: Standing Jump
    }


    public void HandleDodge()
    {
        if ( playerManager.isInteracting ) return;

        if ( inputManager.moveAmount > 0 )
            animatorManager.PlayTargetAnimation("[Airborne] StepSlide Forward", true, true);
        else
            animatorManager.PlayTargetAnimation("[Airborne] StepSlide Backward", true, true);

        // Todo: Left and Right
        // Todo: Toggle invulnerable State
    }
}