using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //upgrades
    [System.NonSerialized] public float numberOfJumps = 0;
    [System.NonSerialized] public float speed = 5.0f;
    [System.NonSerialized] public float jumpHeight = 5.0f;
    [System.NonSerialized] public bool canMultiJump = false;
    [System.NonSerialized] public float health = 100;
    [System.NonSerialized] public float maxHealth = 100;

    //Settings
    public LayerMask groundMask;
    public LayerMask hologramMask;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90.0f;

    //Helper Variables
    [System.NonSerialized] public bool grounded = false;
    private float currentJumps = 0;
    private Vector2 rotation = Vector2.zero;
    bool helmetOn = false;

    //Initialzed objects
    private Animator anim;
    private Rigidbody r;
    private Camera playerCamera;
    //public CinemachineVirtualCamera playerCamera;
    private Transform groundCheck;
    private GameObject flashLight;
    private GameObject helmet;
    private GameManager manager;

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
        flashLight = Camera.main.gameObject.transform.Find("Flashlight").gameObject;
        helmet = Camera.main.gameObject.transform.Find("Helmet").gameObject;
        playerCamera = Camera.main;

        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GameObject.Find("FadeCanvas").GetComponent<Animator>().SetTrigger("FadeIn");
            GetManagerData();
        }

        helmet.transform.localEulerAngles = new Vector3(PlayerPrefs.GetInt("helmetOn") == 0 ? -45 : 0, 0, 0);
        helmetOn = PlayerPrefs.GetInt("helmetOn") == 1 ? true : false;
    }

    //Recieves initial data from the game manager
    private void GetManagerData()
    {
        numberOfJumps = manager.multiJumpAmount;
        canMultiJump = manager.canMultiJump;
        jumpHeight = manager.jumpHeight;
        health = manager.health;
        maxHealth = manager.maxHealth;
        speed = manager.walkSpeed;
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

        if(Time.timeScale == 0)
        {
            GetComponent<AudioSource>().Stop();
            r.linearVelocity = Vector3.zero;
        }
        else
        {
            PlayerRotation();

            AnimateMovement();

            SetGrounded();

            HelmetAnim();

            LookingAt();

            FindHologramObject();

            //set cameras FOV to settings FOV
            Camera.main.fieldOfView = PlayerPrefs.GetInt("FOV");
            GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = Camera.main.fieldOfView;

            //set Sensitivity to settings
            lookSpeed = 10 * PlayerPrefs.GetInt("sens") / 100.0f;
        }


        if(GameObject.Find("GameManager") != null)
            GetManagerData();

    }

    //Called every physics frame

    public Vector3 targetVelocity = Vector3.zero;
    void FixedUpdate()
    {
        Vector3 forwardDir = Vector3.Cross(transform.up, -playerCamera.transform.right).normalized;
        Vector3 rightDir = Vector3.Cross(transform.up, playerCamera.transform.forward).normalized;
        if (grounded)
        {
            targetVelocity = (forwardDir * Input.GetAxis("Vertical") + rightDir * Input.GetAxis("Horizontal")) * speed;
        }
        else
        {
            targetVelocity += (forwardDir * Input.GetAxis("Vertical") + rightDir * Input.GetAxis("Horizontal")) * Time.deltaTime * 20;
        }
        if(targetVelocity.magnitude > speed)
        {
            targetVelocity = targetVelocity.normalized * speed;
        }
        r.MovePosition(transform.position + targetVelocity * Time.deltaTime);


    }

    //Sets the animation perameters
    private void AnimateMovement()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
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
        playerCamera.transform.localEulerAngles = Vector3.right * rotation.x;
        playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, 0, 0);
        Quaternion localRotation = Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        transform.rotation *= localRotation;
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
            r.linearVelocity = Vector3.zero;
            r.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            anim.SetTrigger("jump");
            currentJumps++;
        }
    }

    //Flashlight controls with Input System
    private void FlashLightControl(InputAction.CallbackContext context)
    {
        GameObject.Find("ClickSound").GetComponent<AudioSource>().Play();
        flashLight.SetActive(!flashLight.activeSelf);
    }

    private void HelmetChange(InputAction.CallbackContext context)
    {
        helmetOn = !helmetOn;
        PlayerPrefs.SetInt("helmetOn", helmetOn ? 1 : 0);
    }

    private void HelmetAnim()
    {
        if(helmetOn)
        {
            
            if (helmet.transform.localEulerAngles.x > 3)
            {
                helmet.transform.localEulerAngles += Vector3.right * Time.deltaTime * 60;
            }
            else
            {
                helmet.transform.localEulerAngles = new Vector3(0, helmet.transform.localEulerAngles.y, helmet.transform.localEulerAngles.z);
            }
        }
        else
        {
            if (helmet.transform.localEulerAngles.x < 310 || helmet.transform.localEulerAngles.x > 315)
            {
                helmet.transform.localEulerAngles -= Vector3.right * Time.deltaTime * 60;
            }
            else
            {
                helmet.transform.localEulerAngles = new Vector3(312.5f, helmet.transform.localEulerAngles.y, helmet.transform.localEulerAngles.z);
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
            if(currentObject != null && hit.transform.gameObject != currentObject)
            {
                if (currentObject.GetComponent<MineableObject>() != null)
                {
                    MineableObject obj = currentObject.GetComponent<MineableObject>();
                    hologramParent.transform.Find(obj.itemDrop).gameObject.SetActive(false);
                }
                else if (currentObject.GetComponent<AnimalInfoScript>() != null)
                {
                    AnimalInfoScript obj = currentObject.GetComponent<AnimalInfoScript>();
                    hologramParent.transform.Find(obj.animalName).gameObject.SetActive(false);
                }
            }
            currentObject = hit.transform.gameObject;
        }
        else
        {
            currentObject = null;
        }
    }

}