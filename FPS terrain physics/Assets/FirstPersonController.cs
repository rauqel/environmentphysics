using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;

    [Header("Functional Booleans")]
    private bool canSprint;
    private bool canJump;

    [Header("Control Keys")]
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode jumpKey = KeyCode.Space;

    [Header("Terrain Parameters")]
    [SerializeField] private float grassLowerDivision = 1.0f;
    [SerializeField] private float grassUpperDivision = 2.0f;
    private bool isOnGrass;
    //
    [SerializeField] private float sandLowerDivision = 1.75f;
    [SerializeField] private float sandUpperDivision = 3;
    private bool isOnSand;
    //
    [SerializeField] private float snowLowerDivision = 1.4f;
    [SerializeField] private float snowUpperDivision = 2.5f;
    private bool isOnSnow;
    //
    private float energyTimer;

    [Header("Movement Parameters")]
    private float currentSpeed;
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 75.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpForce = 8.0f;
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;

    [Header("Headbob Parameters")]
    private float bobSpeed;
    private float bobAmount;
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    private float defaultYPos = 0;
    private float bobTimer;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultYPos = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            HandleHeadBob();

            if (canJump)
            {
                HandleJump();
            }

            ApplyFinalMovements();
        }

        currentSpeed = (isSprinting ? sprintSpeed : walkSpeed);

        bobSpeed = (isSprinting ? sprintBobSpeed : walkBobSpeed);
        bobAmount = (isSprinting ? sprintBobAmount : walkBobAmount);
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(currentSpeed * Input.GetAxis("Vertical"), currentSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            canJump = false;
            if(Input.GetKeyDown(sprintKey))
                canSprint = false;
        }
        else
        {
            canSprint = true;
            canJump = true;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;
        if (Mathf.Abs(moveDirection.x) > 0.1 || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(bobTimer) * bobAmount, playerCamera.transform.localPosition.z);
        }
    }

    private void HandleJump()
    {
        if (shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }
}
