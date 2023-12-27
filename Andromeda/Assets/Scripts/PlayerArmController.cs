using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArmController: MonoBehaviour
{
    //Mining States
    public int miningForce;
    public int miningDamage;

    //Attack States
    public int damage;
    public float attackSpeed;

    //helping variables
    private int currentMode = 0;
    private string[] modes = { "mining", "attack" };
    private bool isFiring;

    //Input Actions
    public PlayerControls controls;
    private InputAction fire;
    
    //Assigned variablews
    public LayerMask miningMask;
    public Transform armPosition;
    public LineRenderer line;
    private GameObject mineParticles;
    private GameManager manager;
    private PlayerController controller;

    //When the game first starts
    private void Awake()
    {
        controls = new PlayerControls();
        armPosition = Camera.main.transform.Find("Arm");
        line = this.gameObject.GetComponent<LineRenderer>();
        mineParticles = armPosition.Find("MineParticles").gameObject;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //When the input actions are enabled
    private void OnEnable()
    {
        fire = controls.Player.Fire;
        fire.Enable();
        fire.started += Fire;
        fire.performed += Fire;
        fire.canceled += Fire;
    }

    //When the input actions are disabled
    private void OnDisable()
    {
        fire.Disable();   
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            currentMode += (int) Input.GetAxis("Mouse ScrollWheel");
            currentMode %= Mathf.Abs(modes.Length);
        }

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, miningMask))
        {
            controller.currentObject = hit.transform.gameObject;
        }
        else
        {
            controller.currentObject = null;
        }

        if(isFiring)
        {
            if (currentMode == 0)
            {
                Mine();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            DrawRay(new Vector3(0, 0, 0));
        }
    }

    //activated whenever the fire key is held down
    private void Fire(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case (InputActionPhase.Started):
                isFiring = true;
                break;
            case (InputActionPhase.Canceled):
                isFiring = false;
                break;
        }
    }

    //activated when the user is mining
    private void Mine()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, miningMask))
        {
            MineableObject obj = hit.transform.gameObject.GetComponent<MineableObject>();
            if(obj.hardness <= miningForce)
            {
                obj.currentHealth -= miningDamage * Time.deltaTime;
            }
            DrawRay(hit.point);
            mineParticles.transform.position = hit.point;
            mineParticles.SetActive(true);
        }
        else
        {
            DrawRay(Camera.main.transform.position + Camera.main.transform.forward * 2);
            mineParticles.SetActive(false);
        }
    }

    //activated when the user is attacking
    private void Attack()
    {
    }

    //Draws the ray from the arm to the center of the screnn
    private void DrawRay(Vector3 endLocation)
    {
        if (isFiring)
            line.SetPositions(new Vector3[] { armPosition.transform.position, endLocation});
        else
        {
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            mineParticles.SetActive(false);
        }
            
    }
}
