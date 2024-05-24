using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetGravityTEST : MonoBehaviour
{
    //Set these variables
    private Transform planet;


    //Called right before game starts

    private void Awake()
    {
        planet = GameObject.Find("planet").transform;

        Vector3 toCenter = planet.position - transform.position;
        toCenter.Normalize();
        AlignToPlanet(toCenter);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.zero - transform.position, out hit))
        {
            transform.position = hit.point;
        }
    }

    //Aligns the object to the center of the planet
    private void AlignToPlanet(Vector3 toCenter)
    {
        Quaternion q = Quaternion.FromToRotation(transform.up, -toCenter);
        q = q * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
    }
}
