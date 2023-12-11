using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private Rigidbody rb;

    private float pitch;
    private float yaw;
    private float roll;

    public float responsiveness;
    private ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        pitch = Input.GetAxis("Vertical");
        roll = -Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * 30;
        rb.AddTorque(transform.right * pitch * responsiveness);
        rb.AddTorque(transform.up * yaw * responsiveness);
        rb.AddTorque(transform.forward * roll * responsiveness);

    }

    private void OnTriggerEnter(Collider other)
    {
        explosion.Play();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
