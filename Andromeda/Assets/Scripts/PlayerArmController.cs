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

    private int currentMode = 0;

    private string[] modes = { "mining", "attack" };

    public PlayerControls controls;
    private InputAction fire;
    private bool isFiring;

    public LayerMask miningMask;

    public Transform armPosition;
    public LineRenderer line;

    private void Awake()
    {
        controls = new PlayerControls();
        armPosition = this.transform.Find("Arm");
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        fire = controls.Player.Fire;
        fire.Enable();
        fire.started += Fire;
        fire.performed += Fire;
        fire.canceled += Fire;
    }

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
        DrawRay();
    }

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

    private void Mine()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, miningMask))
        {
            MineableObject obj = hit.transform.gameObject.GetComponent<MineableObject>();
            if(obj.hardness <= miningForce)
            {
                obj.health -= miningDamage * Time.deltaTime;
            }
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }

    private void DrawRay()
    {
        if (isFiring)
            line.SetPositions(new Vector3[] { armPosition.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 2 });
        else
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }
}
