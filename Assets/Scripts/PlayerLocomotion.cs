using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

    private AnimatorManager _animatorManager;
    private PlayerManager _playerManager;
    private InputManager _inputManager;
    
    public Rigidbody playerRigidbody;

    private Vector3 _moveDirection;
    private Camera _mainCamera;

    [Header("Falling")]
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
        if ( Camera.main != null ) _mainCamera = Camera.main;
        else Debug.LogWarning("[Not Assigned]: Camera");

        _animatorManager = GetComponentInChildren<AnimatorManager>();

        _playerManager = GetComponent<PlayerManager>();
        _inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }



    public void HandleMovement()
    {
        Transform currentTransform = transform;
        Vector3 positionToMove = Vector3.zero;
        Vector3 horizontalMovement = currentTransform.right * _inputManager.horizontalInput * Time.fixedDeltaTime;
        Vector3 verticalMovement = currentTransform.forward * _inputManager.verticalInput * Time.fixedDeltaTime;

        if ( _playerManager.isSprinting == false )
        {
            horizontalMovement *= runningSpeed;
            verticalMovement *= runningSpeed;
        }
        else
        {
            horizontalMovement = Vector3.zero;
            verticalMovement *= sprintingSpeed;
        }

        positionToMove = currentTransform.position + horizontalMovement + verticalMovement;
        positionToMove.y = 0;

        playerRigidbody.MovePosition(positionToMove);
        _animatorManager.UpdateAnimatorValues(_inputManager.horizontalInput, _inputManager.verticalInput, _playerManager.isSprinting);
    }

    public void HandleRotation()
    {
        if ( _inputManager.horizontalInput != 0 || (_inputManager.verticalInput < 0) ) return;

        Vector3 targetDirection = _mainCamera.transform.forward * _inputManager.verticalInput;
        targetDirection += _mainCamera.gameObject.transform.right * _inputManager.horizontalInput;
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
        if ( _inputManager.jumpInput )
        {
            _animatorManager.PlayTargetAnimation("JumpFull", true);
            Debug.LogWarning("[ToDO]: HandleJumping properly.");
            // Vector3 currentPosition = transform.up;
            // currentPosition *= _animatorManager.animator.pivotPosition.y; 
            // playerRigidbody.MovePosition(currentPosition);
        }
    }


    public void HandleDodge()
    {
        Debug.LogWarning("[ToDO]: HandleDodge");
    }
}