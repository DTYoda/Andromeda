using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    //Set these variables
    public Transform planet;
    public bool alignToPlanet = true;
    public float gravityConstant = 9.8f;

    //Initialized variables
    Rigidbody r;

    //Called right before game starts
    void Start()
    {
        //Initialzed needed variables
        r = GetComponent<Rigidbody>();
    }

    //called every physics frame
    void FixedUpdate()
    {
        //get the vector from the player to the center of the planet
        Vector3 toCenter = planet.position - transform.position;
        toCenter.Normalize();

        Gravity(toCenter);
        
        if (alignToPlanet)
        {
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
    }
}
