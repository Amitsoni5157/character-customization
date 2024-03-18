using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    [SerializeField]Animator animator;

    Quaternion targetRotation;

    CameraController cameraController;
    // Start is called before the first frame update
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
       // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime;

        var moveDir = cameraController.PlanarRotation * movement;

        if (moveAmount > 0) {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }

         transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveamount", moveAmount);
        
    }
}
