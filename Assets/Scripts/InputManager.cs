using UnityEngine;

public class InputManager : MonoBehaviour {
    private ThirdPersonControls _playerControls;
    private PlayerManager _playerManager;
    private CameraManager _cameraManager;

    public Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;

    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    public bool sprintInput;
    public bool jumpInput;
    public bool dodgeInput;

    /** Lock On **/
    public bool lockOnInput;
    public bool lockOnFlag;
    public bool rightStickLeftInput;
    public bool rightStickRightInput;

    private void Awake()
    {
        // todo: just import player manager
        _playerManager = GetComponent<PlayerManager>();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnEnable()
    {
        if ( _playerControls == null )
        {
            _playerControls = new ThirdPersonControls();

            _playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            _playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // while you hold it --> true!
            _playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            _playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            // when you press the button --> True
            _playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            _playerControls.PlayerActions.Jump.canceled += i => jumpInput = false;
            
            _playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
        }
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
    }

    private void HandleSprintingInput()
    {
        if ( sprintInput && verticalInput > 0.5f )
        {
            _playerManager.isSprinting = true;
        }
        else
        {
            _playerManager.isSprinting = false;
        }
    }
}