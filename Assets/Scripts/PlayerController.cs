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


    [SerializeField]Animator animator;

    Quaternion targetRotation;

    CameraController cameraController;
    CharacterController characterController;
    // Start is called before the first frame update
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        // animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded)
            {
                StartCoroutine(JumpAnimation());
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(isGrounded)
            {
                CrouchAnimation(true);
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            CrouchAnimation(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isGrounded)
            {
                SlideAnimation();
            }
        }/*
        else if (Input.GetKeyUp("F"))
        {
            SlideAnimation();
        }*/
    }

    private void Move()
    {
        WalkAnimation(true);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime;

        var moveDir = cameraController.PlanarRotation * movement;

        GroundCheck();
        Debug.Log("IsGrounded = "+isGrounded);
        if(isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0) {            
            targetRotation = Quaternion.LookRotation(moveDir);
        }

         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveamount", moveAmount, 0.2f, Time.deltaTime);        
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius,groundLayer);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }


    IEnumerator JumpAnimation()
    {
        animator.SetBool("isJump", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("isJump", false);
    }

    void CrouchAnimation(bool isValue)
    {
        animator.SetBool("isCrouch",isValue);        
    }

    void SlideAnimation()
    {
        animator.SetTrigger("isSlide");
    }

    void WalkAnimation(bool isValue)
    {
        animator.SetBool("isWalk", isValue);
    }
}
