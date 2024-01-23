using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    //Set these variables
    private Transform planet;
    public bool alignToPlanet = true;
    public bool useGravity = true;
    public float gravityConstant = 9.8f;
    public GameObject hologram;
    public bool useHologram;

    //Initialized variables
    Rigidbody r;

    //Called right before game starts
    void Start()
    {
        //Initialzed needed variables
        if(GameObject.Find("planet") != null)
            planet = GameObject.Find("planet").transform;
        if(useGravity)
        {
            r = GetComponent<Rigidbody>();
            r.useGravity = false;
        }
            
    }

    //called every physics frame
    void FixedUpdate()
    {
        //get the vector from the player to the center of the planet
        if(useGravity)
        {
            Vector3 toCenter = planet.position - transform.position;
            toCenter.Normalize();
            Gravity(toCenter);
        }

        if (alignToPlanet)
        {
            Vector3 toCenter = planet.position - transform.position;
            toCenter.Normalize();
            AlignToPlanet(toCenter);
        }
    }

    //Adds force of gravity towards the planet
    private void Gravity(Vector3 toCenter)
    {
        r.AddForce(toCenter * gravityConstant, ForceMode.Acceleration);
    }

    //Aligns the object to the center of the planet
    private void AlignToPlanet(Vector3 toCenter)
    {
        Quaternion q = Quaternion.FromToRotation(transform.up, -toCenter);
        q = q * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
        if(useHologram)
            hologram.transform.localEulerAngles = new Vector3(transform.up.x * -180 - 90, transform.up.y * -180, transform.up.z * -180);


    }
}
