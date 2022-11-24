using _Scripts__3rdPersonController;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

    private PlayerManager _playerManager;
    private AnimatorManager _animatorManager;
    private InputManager _inputManager;

    private Vector3 _moveDirection;
    private Camera mainCamera;
    public Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    [SerializeField] private float leapingVelocity = 2.5f;
    [SerializeField] private float fallingVelocity = 35f;
    [SerializeField] private float rayCastHeightOffSet = 0.5f;
    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Speeds")]
    [SerializeField] private float walkingSpeed = 2.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump Speeds")]
    [SerializeField] private float gravityIntensity = -10f;
    [SerializeField] private float jumpHeight = 3f;

    private void Awake()
    {
        if ( Camera.main != null ) mainCamera = Camera.main;
        else Debug.LogWarning("[Not Assigned]: Camera");

        _playerManager = GetComponent<PlayerManager>();
        _inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();

        _animatorManager = GetComponentInChildren<AnimatorManager>();
    }



    public void HandleMovement()
    {
        Debug.LogWarning("[ToDO]:HandleMovement ");

        var myTransform = transform;
        var positionToMove = myTransform.position +
                             myTransform.right * (_inputManager.horizontalInput * Time.fixedDeltaTime * runningSpeed) +
                             myTransform.forward * (_inputManager.verticalInput * Time.fixedDeltaTime * runningSpeed);

        if ( _playerManager.isSprinting )
        {
            positionToMove = myTransform.position +
                             myTransform.right * (_inputManager.horizontalInput * Time.fixedDeltaTime * runningSpeed) +
                             myTransform.forward * (_inputManager.verticalInput * Time.fixedDeltaTime * sprintingSpeed);
        }

        positionToMove.y = 0;

        _animatorManager.UpdateAnimatorValues(_inputManager.horizontalInput, _inputManager.verticalInput, _playerManager.isSprinting);

        playerRigidbody.MovePosition(positionToMove);

    }

    public void HandleRotation()
    {
        if ( _inputManager.horizontalInput != 0 || (_inputManager.verticalInput < 0) ) return;

        Vector3 targetDirection = mainCamera.transform.forward * _inputManager.verticalInput;
        targetDirection += mainCamera.gameObject.transform.right * _inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if ( targetDirection == Vector3.zero )
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        playerRigidbody.MoveRotation(playerRotation);

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