using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject planet;
    public Rigidbody playerBody;
    public float movementSpeed = 4;
    public float originalMovementSpeed = 4;
    public float jumpHeight = 4;
    public float sprintSpeed = 7;
    private bool isSprinting;
    public float gravity = 9.8f;
    public float gravitySpeed;
    private Vector3 direction;

    public LayerMask groundMask;
    public GameObject groundCheck;
    public float checkRadius;

    private bool isGrounded;
    public bool gravityTrue;

    private void Start()
    {
        playerBody = transform.GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        playerBody.MovePosition(playerBody.transform.localPosition+ direction * Time.deltaTime);

        RotateTowardsPlanet();
        PlanetGravity();

    }
    private void Update()
    {
        direction = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");


        if (direction.magnitude > 1)
        {
            direction /= direction.magnitude;
        }
        direction *= movementSpeed;
        direction = playerBody.rotation * direction;

        isGrounded = Physics.CheckSphere(groundCheck.transform.position, checkRadius, groundMask);

        if (isSprinting)
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = originalMovementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerBody.AddForce(new Vector3(0, jumpHeight, 0));
        }
    }

    private void RotateTowardsPlanet()
    {
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, -transform.position + planet.transform.position) * transform.rotation;
        transform.eulerAngles = orientationDirection.eulerAngles;
    }

    private void PlanetGravity()
    {
        if(!isGrounded && gravityTrue == true) { playerBody.AddForce((-transform.position + planet.transform.position).normalized * gravity * playerBody.mass); }
        if(isGrounded && direction == Vector3.zero)
        {
            playerBody.velocity = Vector3.zero;
        }
    }
}
