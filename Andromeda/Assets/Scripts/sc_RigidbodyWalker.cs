using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_RigidbodyWalker : MonoBehaviour
{
    //upgrades
    public float numberOfJumps = 1;
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;

    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    private float currentJumps = 0;

    bool grounded = false;
    Rigidbody r;
    Vector2 rotation = Vector2.zero;
    float maxVelocityChange = 10.0f;

    private Animator anim;
    private Transform groundCheck;
    public LayerMask groundMask;

    //called as game starts
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        groundCheck = transform.Find("GroundCheck");
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
    }

    //Called once every frame
    void Update()
    {
        PlayerRotation();

        AnimateMovement();

        if (Input.GetButtonDown("Jump") && currentJumps < numberOfJumps)
        {
            r.velocity = Vector3.zero;
            r.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            anim.SetTrigger("jump");
            currentJumps++;
        }

        grounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if(grounded)
        {
            currentJumps = 0;
        }

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
}