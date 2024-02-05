using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //upgrades
    [System.NonSerialized] public float numberOfJumps = 0;
    [System.NonSerialized] public float speed = 5.0f;
    [System.NonSerialized] public float jumpHeight = 5.0f;
    [System.NonSerialized] public bool canMultiJump = false;
    [System.NonSerialized] public float health = 100;

    //Settings
    public LayerMask groundMask;
    public LayerMask hologramMask;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90.0f;

    //Helper Variables
    [System.NonSerialized] public bool grounded = false;
    private float currentJumps = 0;
    private Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;
    bool helmetOn = true;

    //Initialzed objects
    private Animator anim;
    private Rigidbody r;
    private Camera playerCamera;
    private Transform groundCheck;
    private GameObject flashLight;
    private GameObject helmet;

    //Input System
    public PlayerControls controls;
    private InputAction flashLightInput;
    private InputAction jumpInput;
    private InputAction helmetInput;

    //Current Object Hologram and text
    public GameObject hologramParent;
    public GameObject currentObject;
    public GameObject objHologram;
    public TMP_Text hologramText;
    public GameObject objHealthBar;

    //called as game starts, initializes needed items
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        groundCheck = transform.Find("GroundCheck");
        playerCamera = Camera.main;
        flashLight = playerCamera.gameObject.transform.Find("Flashlight").gameObject;
        helmet = playerCamera.gameObject.transform.Find("Helmet").gameObject;

        if(GameObject.Find("GameManager") != null)
            GameObject.Find("GameManager").GetComponent<Animator>().SetTrigger("FadeIn");
    }

    //Called right before game starts
    void Awake()
    {
        //set up rigidbody for proper use
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;
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

        helmetInput = controls.Player.Helmet;
        helmetInput.Enable();
        helmetInput.performed += HelmetChange;
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

        HelmetAnim();

        LookingAt();

        FindHologramObject();

    }

    //Called every physics frame

    public Vector3 targetVelocity = Vector3.zero;
    void FixedUpdate()
    {
        if (true)
        {
            // Calculate how fast we should be moving
            Vector3 forwardDir = Vector3.Cross(transform.up, -playerCamera.transform.right).normalized;
            Vector3 rightDir = Vector3.Cross(transform.up, playerCamera.transform.forward).normalized;
            targetVelocity = (forwardDir * Input.GetAxis("Vertical") + rightDir * Input.GetAxis("Horizontal")) * speed;

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

    private void HelmetChange(InputAction.CallbackContext context)
    {
        helmetOn = !helmetOn;
    }

    private void HelmetAnim()
    {

        if(helmetOn)
        {
            if (helmet.transform.localEulerAngles.x > 3)
            {
                helmet.transform.localEulerAngles += Vector3.right * Time.deltaTime * 60;
            }
        }
        else
        {
            if (helmet.transform.localEulerAngles.x < 265 || helmet.transform.localEulerAngles.x > 275)
            {
                helmet.transform.localEulerAngles -= Vector3.right * Time.deltaTime * 60;
            }
        }
    }

    private void LookingAt()
    {
        if (currentObject != null)
        {
            if(currentObject.GetComponent<MineableObject>() != null)
            {
                MineableObject obj = currentObject.GetComponent<MineableObject>();
                hologramText.text = "Name: " + obj.objName + "\nHardness: " + obj.hardness + "\nDrop: " + obj.itemDrop;
                objHologram = hologramParent.transform.Find(obj.itemDrop).gameObject;
                objHologram.SetActive(true);
                objHealthBar.SetActive(true);
                objHologram.transform.localEulerAngles += Vector3.up * Time.deltaTime * 10;
                objHealthBar.transform.GetChild(0).localScale = new Vector3(obj.currentHealth / obj.totalHealth, 1, 1);
            }
            else if(currentObject.GetComponent<AnimalInfoScript>() != null)
            {
                AnimalInfoScript obj = currentObject.GetComponent<AnimalInfoScript>();
                hologramText.text = "Name: " + obj.animalName + "\nType: " + obj.animalType + "\nDrop: " + obj.animalDrop;
                objHologram = hologramParent.transform.Find(obj.animalName).gameObject;
                objHologram.SetActive(true);
                objHealthBar.SetActive(true);
                objHologram.transform.localEulerAngles += Vector3.up * Time.deltaTime * 10;
                objHealthBar.transform.GetChild(0).localScale = new Vector3(obj.currentHealth / obj.totalHealth, 1, 1);
            }
        }
        else
        {
            if (objHologram != null)
            {
                objHologram.SetActive(false);
            }
            hologramText.text = "";
            objHealthBar.SetActive(false);
        }
    }

    private void FindHologramObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10, hologramMask))
        {
            currentObject = hit.transform.gameObject;
        }
        else
        {
            currentObject = null;
        }
    }

}