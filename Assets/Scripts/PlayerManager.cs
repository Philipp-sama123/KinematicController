using _Scripts__3rdPersonController;
using UnityEngine;

// calls and runs everything 
//runs all functionality for the player
public class PlayerManager : CharacterManager {

    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private AnimatorManager _animatorManager;
    private PlayerLocomotion _playerLocomotion;

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
        _inputManager = GetComponent<InputManager>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();
        _animatorManager = GetComponentInChildren<AnimatorManager>();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        _inputManager.HandleAllInputs();


    }

    private void FixedUpdate()
    {
        _playerLocomotion.HandleMovement();
        _playerLocomotion.HandleRotation();

    }

    private void LateUpdate()
    {

        float deltaTime = Time.deltaTime;

        if ( _cameraManager != null )
            _cameraManager.HandleAllCameraMovement(deltaTime, _inputManager.cameraInputX, _inputManager.cameraInputY);
        else Debug.LogWarning("[Error] No Camera found!");


    }
}