using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;

    [Header("References")]
    //

    [Header("Functional Booleans")]
    private bool canSprint;
    private bool canJump;

    [Header("Control Keys")]
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode jumpKey = KeyCode.Space;

    [Header("UI control")]
    [SerializeField] private Image energyBar;

    [Header("Movement Parameters")]
    private float currentSpeed;
    private float walkSpeed = 4.5f;
    private float sprintSpeed = 9f;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    //
    private bool isMoving;

    [Header("Terrain Parameters")]
    private float currentLowerDivision, currentUpperDivision, currentDivision;
    [SerializeField] private float grassLowerDivision = 1.0f;
    [SerializeField] private float grassUpperDivision = 2.0f;
    public bool isOnGrass;
    //
    [SerializeField] private float sandLowerDivision = 1.75f;
    [SerializeField] private float sandUpperDivision = 3;
    public bool isOnSand;
    //
    [SerializeField] private float snowLowerDivision = 1.4f;
    [SerializeField] private float snowUpperDivision = 2.5f;
    public bool isOnSnow;
    //
    private float energyTimer;
    private bool canChange;
    //
    [SerializeField] private LayerMask grassFloor;
    [SerializeField] private LayerMask snowFloor;
    [SerializeField] private LayerMask sandFloor;

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
    [SerializeField] private GameObject playerObject;
    private Rigidbody playerRB;
    private CharacterController characterController;


    private float currentVel;
    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    void Awake()
    {
        playerRB = playerObject.GetComponent<Rigidbody>();

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultYPos = playerCamera.transform.localPosition.y;

        currentVel = playerRB.sleepThreshold;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            HandleTerrainMovement();

            HandleHeadBob();
            HandleUI();

            if (canJump)
            {
                HandleJump();
            }

            ApplyFinalMovements();
        }

        currentSpeed = (isOnGrass || isOnSnow || isOnSand ? (isSprinting ? sprintSpeed / currentDivision : walkSpeed / currentDivision) :
            (isSprinting ? sprintSpeed : walkSpeed));

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

    private void HandleTerrainMovement()
    {
        isOnGrass = Physics.Raycast(transform.position, Vector3.down,  2 * 0.5f + 0.2f, grassFloor);
        isOnSnow = Physics.Raycast(transform.position, Vector3.down, 2 * 0.5f + 0.2f, snowFloor);
        isOnSand = Physics.Raycast(transform.position, Vector3.down, 2 * 0.5f + 0.2f, sandFloor);

        //
        if (isOnGrass)
        {
            isOnSnow = false;
            isOnSand = false;
            currentLowerDivision = grassLowerDivision;
            currentUpperDivision = grassUpperDivision;
        }
        if (isOnSand)
        {
            isOnSnow = false;
            isOnGrass = false;
            currentLowerDivision = sandLowerDivision;
            currentUpperDivision = sandUpperDivision;
        }
        if (isOnSnow)
        {
            isOnGrass = false;
            isOnSand = false;
            currentLowerDivision = snowLowerDivision;
            currentUpperDivision = snowUpperDivision;
        }
    }

    private void CalculateTerrainSpeeds()
    {
        if (isMoving)
        {
            currentDivision = currentLowerDivision;
            currentDivision += Time.deltaTime / (isSprinting ? 30 : 70);
            if (currentDivision >= currentUpperDivision)
                currentDivision = currentUpperDivision;
        }
        if (!isMoving)
        {
            currentDivision -= Time.deltaTime / 140;
            if (currentDivision <= currentLowerDivision)
                currentDivision = currentLowerDivision;
        }
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

    private void HandleUI()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            isMoving = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;
        }

        if (isMoving)
        {
            energyBar.fillAmount -= Time.deltaTime / (isSprinting ? 30 : 70);
        }
        else
        {
            if (energyBar.fillAmount != 1)
            {
                energyTimer += Time.deltaTime;
            }
            else
            {
                energyTimer = 0;
            }

            if (energyTimer > 3f)
            {
                energyBar.fillAmount += Time.deltaTime / 140;
            }
        }
    }
}
