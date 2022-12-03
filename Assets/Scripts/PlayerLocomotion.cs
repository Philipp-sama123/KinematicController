using System.Collections;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour {

    private AnimatorManager _animatorManager;
    private PlayerManager _playerManager;
    private InputManager _inputManager;

    public new Rigidbody  rigidbody;

    private Vector3 _moveDirection;
    private Camera _mainCamera;

    [Header("Falling")]
    [SerializeField] private float leapingVelocity = 2.5f;
    [SerializeField] private float fallingVelocity = 35f;
    [SerializeField] private float rayCastHeightOffSet = 0.5f;
    [SerializeField] private float distanceForGroundCheck = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Speeds")]
    [SerializeField] private float walkingSpeed = 2.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float sprintingSpeed = 7.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector3 moveDirection;

    [Header("Jump Speeds")]
    [SerializeField] private float fallingSpeed = -10f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float jumpSpeed = 5f;

    private void Awake()
    {
        if ( Camera.main != null ) _mainCamera = Camera.main;
        else Debug.LogWarning("[Not Assigned]: Camera");

        _animatorManager = GetComponent<AnimatorManager>();
        _playerManager = GetComponent<PlayerManager>();
        _inputManager = GetComponent<InputManager>();
        rigidbody = GetComponent<Rigidbody>();
    }



    public void HandleMovement()
    {
        Vector3 normalVector = transform.up; // ToDo !
        Vector3 targetPosition;

        // if ( _playerManager.isGrounded == false ) return;
        moveDirection = _mainCamera.transform.forward * _inputManager.verticalInput;
        moveDirection += _mainCamera.transform.right * _inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = runningSpeed;

        if ( _inputManager.dodgeAndSprintInput && _inputManager.moveAmount > 0.5f )
        {
            speed = sprintingSpeed;
            _playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if ( _inputManager.moveAmount < 0.5f )
            {
                moveDirection *= walkingSpeed;
                _playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                _playerManager.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        _animatorManager.UpdateAnimatorValues(0, _inputManager.moveAmount, _playerManager.isSprinting);

    }

    public void HandleRotation(float deltaTime)
    {
        if ( _inputManager.moveAmount == 0 ) return;
        
        Vector3 targetDir = Vector3.zero;

        targetDir = _mainCamera.transform.forward * _inputManager.verticalInput;
        targetDir += _mainCamera.transform.right * _inputManager.horizontalInput;

        targetDir.Normalize();
        targetDir.y = 0; // no movement on y-Axis (!)

        if ( targetDir == Vector3.zero )
            targetDir = transform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * deltaTime);

        transform.rotation = targetRotation;
    }

    public void HandleFalling()
    {
        if ( _playerManager.isGrounded ) return;

        // Vector3 playerFallingPosition = transform.position;
        // playerFallingPosition.y *=  fallingSpeed * Time.deltaTime; 
        // Debug.Log(playerFallingPosition);
        // playerRigidbody.MovePosition(playerFallingPosition);

        Debug.LogWarning("[ToDO]: HandleFalling");

    }

    public void HandleJumping()
    {
        if ( _inputManager.jumpInput )
        {
            _animatorManager.PlayTargetAnimation("JumpFull", true);
            Debug.LogWarning("[ToDO]: HandleJumping properly.");
        }
    }
    public void HandleRollingAndSprinting()
    {
        // if ( _playerManager.isUsingRootMotion )
        // {
        //     return;
        // }

        // if ( playerStats.currentStamina <= 0 )
        // {
        //     return;
        // }

        if ( _inputManager.dodgeFlag )
        {
            moveDirection = _mainCamera.transform.forward * _inputManager.verticalInput;
            moveDirection += _mainCamera.transform.right * _inputManager.horizontalInput;

            if ( _inputManager.moveAmount > 0 )
            {
                _animatorManager.PlayTargetAnimation("Dodge Forward", true); // Todo: rename Anim
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = rollRotation;
                // playerStats.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                _animatorManager.PlayTargetAnimation("Dodge Backward", true); // Todo: rename Anim
                moveDirection.y = 0;
                // playerStats.TakeStaminaDamage(backStepStaminaCost);

            }
        }

        moveDirection = _mainCamera.transform.forward * _inputManager.verticalInput;
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position +distanceForGroundCheck, distanceForGroundCheck * Vector3.down);
    }

    public void CheckIfGrounded()
    {
        var startingPoint = transform.position;
        startingPoint.y += .5f;
        if ( Physics.Raycast(startingPoint, Vector3.down, distanceForGroundCheck) )
        {
            _playerManager.isGrounded = true;
        }
        else
        {
            _playerManager.isGrounded = false;
        }
    }


    private IEnumerator Jump()
    {
        Vector3 groundPos = transform.position;

        while ( true )
        {
            if ( transform.position.y >= maxJumpHeight )
                _inputManager.jumpInput = false;
            if ( _inputManager.jumpInput )
                transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
            else if ( !_inputManager.jumpInput )
            {
                transform.position = Vector3.Lerp(transform.position, groundPos, fallingSpeed * Time.smoothDeltaTime);
                if ( transform.position == groundPos )
                    StopAllCoroutines();
            }

            yield return new WaitForEndOfFrame();
        }
    }


    public void HandleDodge()
    {
        Debug.LogWarning("[ToDO]: HandleDodge");
    }
}