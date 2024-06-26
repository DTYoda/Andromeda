using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArmController: MonoBehaviour
{
    //stats
    [System.NonSerialized] public float armFuel = 20;
    [System.NonSerialized] public float maxArmFuel = 20;

    //Melee Stats
    public float miningForce;
    public float miningDamage;

    //Ranged Stats
    public float attackSpeed = 1;
    private bool canShoot = true;
    public GameObject attackProjectile;

    //helping variables
    private int currentMode = 0;
    private string[] modes = { "melee", "ranged" };
    private bool isFiring;

    //Input Actions
    public PlayerControls controls;
    private InputAction fire;
    
    //Assigned variablews
    public LayerMask miningMask;
    public LayerMask pickupMask;
    public LayerMask attackMask;
    public Transform armPosition;
    public LineRenderer line;
    public GameObject mineParticles;
    private GameManager manager = null;
    private PlayerController controller;
    public AudioSource miningAudio;
    [System.NonSerialized] public bool isAttacking;

    //When the game first starts
    private void Awake()
    {
        controls = new PlayerControls();
        armPosition = Camera.main.transform.Find("Arm");
        line = this.gameObject.GetComponent<LineRenderer>();
        mineParticles = armPosition.Find("MineParticles").gameObject;
        if (GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            GetManagerData();
        }
            
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        if (manager.playerLocation != Vector3.zero)
            transform.position = manager.playerLocation;
    }

    private void GetManagerData()
    {
        armFuel = manager.armFuel;
        maxArmFuel = manager.maxArmFuel;
        miningDamage = manager.armDamage;
        miningForce = manager.armStrength;
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
            currentMode += (int)Input.GetAxis("Mouse ScrollWheel");
            currentMode %= Mathf.Abs(modes.Length);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = 1;
        }

        if (isFiring && manager.armFuel > 0)
        {
            isAttacking = true;
            if (currentMode == 0)
            {
                Melee();
            }
            else if(canShoot && armFuel >= 1)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            isAttacking = false;
            if(manager.armFuel < 0)
            {
                manager.armFuel = 0;
            }
            DrawRay(new Vector3(0, 0, 0));
        }

        GetManagerData();

        FindPickupObjects();
    }

    //activated whenever the fire key is held down
    private void Fire(InputAction.CallbackContext context)
    {
        if(Time.timeScale == 1)
        {
            switch (context.phase)
            {
                case (InputActionPhase.Started):
                    isFiring = true;
                    break;
                case (InputActionPhase.Canceled):
                    isFiring = false;
                    break;
            }
        }
        
    }

    //activated when the user is mining

    public MineableObject mineObj;
    public AnimalInfoScript animObj;
    private void Melee()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, miningMask))
        {
            mineObj = hit.transform.gameObject.GetComponent<MineableObject>();
            if(mineObj.hardness <= miningForce)
            {
                mineObj.currentHealth -= miningDamage * Time.deltaTime;
            }
            DrawRay(hit.point);
            mineParticles.transform.position = hit.point;
            mineParticles.SetActive(true);
            animObj = null;
        }
        else if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, attackMask))
        {
            animObj = hit.transform.gameObject.GetComponent<AnimalInfoScript>();
            animObj.currentHealth -= miningDamage * Time.deltaTime;
            DrawRay(hit.point);
            mineParticles.transform.position = hit.point;
            mineParticles.SetActive(true);
            mineObj = null;
        }
        else
        {
            DrawRay(Camera.main.transform.position + Camera.main.transform.forward * 2);
            mineParticles.SetActive(false);
            mineObj = null;
            animObj = null;
        }
    }

    //activated when the user is attacking
    IEnumerator Shoot()
    {
        canShoot = false;
        BulletScript bullet = Instantiate(attackProjectile, Camera.main.transform.position, Camera.main.transform.rotation).GetComponent<BulletScript>();
        bullet.damage = miningDamage / 2.0f;
        manager.armFuel -= 1;
        yield return new WaitForSeconds(attackSpeed);
        canShoot = true;
    }

    //Draws the ray from the arm to the center of the screnn
    private void DrawRay(Vector3 endLocation)
    {
        if (isFiring)
        {
            line.SetPositions(new Vector3[] { armPosition.transform.position, endLocation });
            miningAudio.Play();
            manager.armFuel -= Time.deltaTime;
        }  
        else
        {
            miningAudio.Stop();
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            mineParticles.SetActive(false);
        }
            
    }

    //Find objects that can be picked up
    public RaycastHit itemHit;
    public GameObject currentPickUp;
    private void FindPickupObjects()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out itemHit, 3, pickupMask))
        {
            currentPickUp = itemHit.transform.gameObject;
            ItemScript obj = itemHit.transform.gameObject.GetComponent<ItemScript>();
            obj.isLooking = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameManager.manager.addItem(obj.itemName, 1);
                transform.Find("PickupSound").gameObject.GetComponent<AudioSource>().Play();
                Destroy(obj.gameObject);
            }
        }
        else
        {
            currentPickUp = null;
        }
    }
}
