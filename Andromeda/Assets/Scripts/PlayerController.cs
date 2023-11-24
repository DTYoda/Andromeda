using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //upgrades
    [System.NonSerialized] public float numberOfJumps = 1;
    [System.NonSerialized] public float speed = 5.0f;
    [System.NonSerialized] public float jumpHeight = 5.0f;
    [System.NonSerialized] public bool canMultiJump = false;

    //Settings
    public LayerMask groundMask;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90.0f;

    //Helper Variables
    bool grounded = false;
    private float currentJumps = 0;
    private Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;

    //Initialzed objects
    private Animator anim;
    private Rigidbody r;
    private Camera playerCamera;
    private Transform groundCheck;
    private GameObject flashLight;

    //Input System
    public PlayerControls controls;
    private InputAction flashLightInput;
    private InputAction jumpInput;

    

    //called as game starts, initializes needed items
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        groundCheck = transform.Find("GroundCheck");
        playerCamera = Camera.main;
        flashLight = playerCamera.gameObject.transform.Find("Flashlight").gameObject;
    }

    //Called right before game starts
    void Awake()
    {
        //set up rigidbody for proper use
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;
        r.useGravity = false;
        r.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rotation.y = transform.eulerAngles.y;

        //set the cursor to be invisible and locked to the center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls = new PlayerControls();
    }

    //Enables all input actions
    private void OnEnable()
    {
        flashLightInput = controls.Player.Flashlight;
        flashLightInput.Enable();
        flashLightInput.performed += FlashLightControl;

        jumpInput = controls.Player.Jump;
        jumpInput.Enable();
        jumpInput.performed += JumpAction;
    }

    //Disables all Input Actions
    private void OnDisable()
    {
        flashLightInput.Disable();
        jumpInput.Disable();
    }

    //Called once every frame
    void Update()
    {
        PlayerRotation();

        AnimateMovement();

        SetGrounded();
    }

    //Called every physics frame
    void FixedUpdate()
    {
        if (true)
        {
            // Calculate how fast we should be moving
            Vector3 forwardDir = Vector3.Cross(transform.up, -playerCamera.transform.right).normalized;
            Vector3 rightDir = Vector3.Cross(transform.up, playerCamera.transform.forward).normalized;
            Vector3 targetVelocity = (forwardDir * Input.GetAxis("Vertical") + rightDir * Input.GetAxis("Horizontal")) * speed;

            Vector3 velocity = transform.InverseTransformDirection(r.velocity);
            velocity.y = 0;
            velocity = transform.TransformDirection(velocity);
            Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            velocityChange = transform.TransformDirection(velocityChange);

            r.AddForce(velocityChange, ForceMode.VelocityChange);
        }


    }

    //Sets the animation perameters
    private void AnimateMovement()
    {
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }

        anim.SetBool("isGrounded", grounded);
    }

    //rotates the player and camera
    private void PlayerRotation()
    {
        rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        Quaternion localRotation = Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        transform.rotation = transform.rotation * localRotation;
    }

    //Set the grounded status
    private void SetGrounded()
    {
        grounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (grounded)
        {
            currentJumps = 0;
        }
    }

    //Called everytime jump input is activated
    private void JumpAction(InputAction.CallbackContext context)
    {
        if ((currentJumps < numberOfJumps && canMultiJump) || (!canMultiJump && grounded))
        {
            r.velocity = Vector3.zero;
            r.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            anim.SetTrigger("jump");
            currentJumps++;
        }
    }

    //Flashlight controls with Input System
    private void FlashLightControl(InputAction.CallbackContext context)
    {
        flashLight.SetActive(!flashLight.activeSelf);
    }

}