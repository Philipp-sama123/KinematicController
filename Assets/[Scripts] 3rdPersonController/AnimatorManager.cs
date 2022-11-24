using _Scripts__3rdPersonController;
using UnityEngine;

public class AnimatorManager : MonoBehaviour {
    public Animator animator;

    private PlayerManager playerManager;
    private Rigidbody playerRigidbody;

    private int Vertical;
    private int Horizontal;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerManager = GetComponent<PlayerManager>();
        playerRigidbody = GetComponent<Rigidbody>();

        Horizontal = Animator.StringToHash("Horizontal");
        Vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;
        #region Snapped Horizontal

        if ( horizontalMovement > 0 && horizontalMovement < 0.55f )
        {
            snappedHorizontal = 0.5f;
        }
        else if ( horizontalMovement > 0.55f )
        {
            snappedHorizontal = 1;
        }
        else if ( horizontalMovement < 0 && horizontalMovement > -0.55f )
        {
            snappedHorizontal = -0.5f;
        }
        else if ( horizontalMovement < -0.55f )
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        #endregion
        #region Snapped Vertical

        if ( verticalMovement > 0 && verticalMovement < 0.55f )
        {
            snappedVertical = 0.5f;
        }
        else if ( verticalMovement > 0.55f )
        {
            snappedVertical = 1;
        }
        else if ( verticalMovement < 0 && verticalMovement > -0.55f )
        {
            snappedVertical = -0.5f;
        }
        else if ( verticalMovement < -0.55f )
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        #endregion

        if ( isSprinting )
        {
            snappedVertical = 2;
            // snappedHorizontal = 2;
        }

        animator.SetFloat(Horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(Vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("IsInteracting", isInteracting);
        animator.SetBool("IsUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }
}