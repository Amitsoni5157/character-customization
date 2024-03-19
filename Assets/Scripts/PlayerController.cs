using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
    float ySpeed;
    Quaternion targetRotation;
    CameraController cameraController;
    CharacterController characterController;
    [SerializeField] Animator animator;

    // Cache frequently accessed components
    Transform cachedTransform;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        cachedTransform = transform;
    }

    void Update()
    {
        Move();

        // Combined input checks
        if (Input.GetKeyDown("space") && isGrounded)
            StartCoroutine(JumpAnimation());

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            CrouchAnimation(true);
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            CrouchAnimation(false);

        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
            SlideAnimation();
    }

    /// <summary>
    /// Character Movement Code Here through WASD and arrow key
    /// </summary>
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.fixedDeltaTime;

        var moveDir = cameraController.PlanarRotation * movement;

        // Using CharacterController's isGrounded
        isGrounded = characterController.isGrounded;
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.fixedDeltaTime;
        }
        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.fixedDeltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
            // Only update rotation when there's actual movement
            cachedTransform.rotation = Quaternion.RotateTowards(cachedTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        animator.SetFloat("moveamount", moveAmount, 0.2f, Time.deltaTime);

    }

    /// <summary>
    /// For Slide Animation
    /// </summary>
    void SlideAnimation()
    {
        animator.SetTrigger("isSlide");
    }

    /// <summary>
    /// For Jump Animation
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpAnimation()
    {
        animator.SetBool("isJump", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("isJump", false);
    }

    /// <summary>
    /// For Crouch Animation
    /// </summary>
    /// <param name="isValue"></param>
    void CrouchAnimation(bool isValue)
    {
        animator.SetBool("isCrouch", isValue);
    }


}
