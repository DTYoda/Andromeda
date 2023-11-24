using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Initialized variables
    private GameObject player;
    private Rigidbody rb;

    //helper variable
    private bool isJumping = false;

    //called right before game starts
    private void Start()
    {
        //initialize all needed variables
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    //called once every frame
    private void Update()
    {
        if(!isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }

    //Called when enemy jumps
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(2);
        rb.AddForce((player.transform.position - transform.position).normalized * 500);
        isJumping = false;
    }
}
