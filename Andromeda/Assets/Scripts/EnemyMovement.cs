using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;

    private Rigidbody rb;
    private bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(!isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(2);
        rb.AddForce((player.transform.position - transform.position).normalized * 500);
        isJumping = false;
    }
}
