using _Scripts__3rdPersonController;
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



    public void HandleMovement()
    {
        Debug.LogWarning("[ToDO]:HandleMovement ");

        animatorManager.UpdateAnimatorValues(0, inputManager.moveAmount, playerManager.isSprinting);

        playerRigidbody.MovePosition(transform.position + new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput) * 5);

    }

    public void HandleRotation()
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
        Debug.LogWarning("[ToDO]: HandleFalling");

    }

    public void HandleJumping()
    {
        Debug.LogWarning("[ToDO]: HandleJumping");
    }


    public void HandleDodge()
    {
        Debug.LogWarning("[ToDO]: HandleDodge");
    }
}