using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geckoScript : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    private Rigidbody rb;
    private float currentSpeed;

    private string state = "idle";
    private bool isIdleAnim = false;
    private bool isRotating = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            StartCoroutine(RunAway());
        }

        
        ScaredAction();
        Move();

        if(state == "idle" && !isIdleAnim)
        {
            StartCoroutine(IdleAction());
        }
        else if (state != "idle")
        {
            StopCoroutine(IdleAction());
        }

        if(isRotating && timer > 0)
        {
            transform.rotation = transform.rotation * localRotation;
            timer -= Time.deltaTime;
        }
        else if(isRotating)
        {
            isRotating = false;
        }


    }

    IEnumerator RunAway()
    {
        state = "scared";

        Quaternion lookTo = Quaternion.FromToRotation(this.transform.forward, this.transform.position - player.transform.position);
        lookTo *= transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookTo, 1);
        currentSpeed = 4;
        yield return new WaitForSeconds(3);
        state = "idle";
    }

    private void ScaredAction()
    {
        if (state == "scared")
        {
            anim.SetBool("scared", true);
            if (GetComponent<Rigidbody>().velocity.magnitude > 0.4f)
            {
                anim.SetBool("run", true);
            }
            else
            {
                anim.SetBool("run", false);
            }
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("scared", false);
        }
    }


    Quaternion localRotation;
    float timer = 0;
    private void RandomRotation()
    {
        isRotating = true;
        localRotation = Quaternion.Euler(0f, Random.Range(-1f,1f), 0f);
        timer = Random.Range(0,2f);
    }

    private IEnumerator IdleAction()
    {
        isIdleAnim = true;
        switch(Random.Range(0, 5))
        {
            case (0):
                currentSpeed = 2;
                break;
            case (1):
                currentSpeed = 1;
                break;
            case (2):
                RandomRotation();
                currentSpeed = 0;
                break;
            case (3):
                RandomRotation();
                currentSpeed = 1;
                break;
        }
        yield return new WaitForSeconds(4);
        isIdleAnim = false;

    }

    private void Move()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if(currentSpeed != 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }
}
