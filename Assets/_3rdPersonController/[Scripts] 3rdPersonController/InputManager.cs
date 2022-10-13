using UnityEngine;

public class InputManager : MonoBehaviour {
    private ThirdPersonControls playerControls;
    private PlayerManager playerManager;

    public Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;

    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;

    public bool sprintInput;
    public bool jumpInput;
    public bool dodgeInput;

    private void Awake()
    {
        // todo: just import player manager
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if ( playerControls == null )
        {
            playerControls = new ThirdPersonControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // while you hold it --> true!
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            // when you press the button --> True
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
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

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    private void HandleSprintingInput()
    {
        if ( sprintInput && moveAmount > 0.5f )
        {
            playerManager.isSprinting = true;
        }
        else
        {
            playerManager.isSprinting = false;
        }
    }
}