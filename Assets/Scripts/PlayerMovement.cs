using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;          // camera locked to player 
    public float walkSpeed = 6f;         // default walk speed
    public float runSpeed = 12f;         // defaukt running speed
    public float jumpPower = 7f;        // defaulkt jump power (how high you are able to jump)
    public float gravity = 10f;         // what the default gravity is
    public float lookSpeed = 2f;       // the sensitivity of the camera
    public float lookXLimit = 45f;     // how far you can look on the axis
    public float defaultHeight = 2f;   // height of capsule
    public float crouchHeight = 1f;    // how low your capsule will go down when crouched
    public float crouchSpeed = 3f;      // movement numbers how fast you crouch
    private float defaultWalkSpeed;
    private float defaultRunSpeed; 

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;   // camera is locked once pressed 
        Cursor.visible = false;    // cursor isnt visible when mouse is clicked
        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);   // directions of capsule

        bool isRunning = Input.GetKey(KeyCode.LeftShift);   // press shift to run
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;     // you are able to run once you are on the ground
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;   // you are able to walk once you are on the ground
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;   // this is the jump power
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)   // once capsule is on the ground
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = defaultWalkSpeed;
            runSpeed = defaultRunSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}