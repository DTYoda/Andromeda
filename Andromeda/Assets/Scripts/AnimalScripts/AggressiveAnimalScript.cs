using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveAnimalScript : MonoBehaviour
{

    public float attackRange = 5;
    public float speed = 2;
    public float damage;
    public float knockBack = 5;

    private Animator anim;
    private GameObject player;
    private float currentSpeed;

    private string state = "idle";
    private bool isIdleAnim = false;
    private bool isRotating = false;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < 50)
        {
            if ((Vector3.Distance(transform.position, player.transform.position) < attackRange) || (Vector3.Distance(transform.position, player.transform.position) < 2 * attackRange && state == "running"))
            {
                if(state != "attacking")
                    StartCoroutine(AttackPlayer());
                Vector3 lookingDir = player.transform.position - this.transform.position;
                lookingDir = transform.InverseTransformDirection(lookingDir);
                lookingDir.y = 0;
                lookingDir = transform.TransformDirection(lookingDir);

                Quaternion lookTo = Quaternion.FromToRotation(this.transform.forward, lookingDir);
                lookTo *= transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookTo, 1);
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > 30 && transform.position.y > 0)
            {
                AnimalSpawner.Instance.spawnedEnemies.Remove(this.transform);
                Destroy(this.gameObject);
            }
            Move();


            if (state == "idle" && !isIdleAnim)
            {
                StartCoroutine(IdleAction());
            }
            else if (state != "idle")
            {
                StopCoroutine(IdleAction());
            }

            if (isRotating && timer > 0)
            {
                transform.rotation = transform.rotation * localRotation;
                timer -= Time.deltaTime;
            }
            else if (isRotating)
            {
                isRotating = false;
            }
        }
        else
        {
            AnimalSpawner.Instance.spawnedEnemies.Remove(this.transform);
            Destroy(this.gameObject);
        }
    }

    IEnumerator AttackPlayer()
    {
        state = "running";
        
        currentSpeed = speed * 2;
        yield return new WaitForSeconds(3);
        state = "idle";
    }

    IEnumerator AttackingReset()
    {
        state = "attacking";
        anim.SetTrigger("attacking");
        yield return new WaitForSeconds(2);
        state = "idle";
    }


    Quaternion localRotation;
    float timer = 0;
    private void RandomRotation()
    {
        isRotating = true;
        localRotation = Quaternion.Euler(0f, Random.Range(-3f,3f), 0f);
        timer = Random.Range(0,2f);
    }

    private IEnumerator IdleAction()
    {
        isIdleAnim = true;
        switch(Random.Range(0, 4))
        {
            case (0):
                currentSpeed = speed;
                break;
            case (1):
                currentSpeed = speed - 1;
                break;
            case (2):
                RandomRotation();
                currentSpeed = 0;
                break;
            case (3):
                RandomRotation();
                currentSpeed = speed - 1;
                break;
        }
        yield return new WaitForSeconds(4);
        isIdleAnim = false;

    }

    private void Move()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if(currentSpeed != 0 && state != "attacking")
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (state == "running")
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(AttackingReset());
            if(player.GetComponent<PlayerController>().grounded)
            {
                player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                player.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(new Vector3(0, 1, 1)) * knockBack, ForceMode.Impulse);
                GameObject.Find("GruntSound").GetComponent<AudioSource>().Play();
            }   
            if(GameObject.Find("GameManager") != null)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().health -= damage;
            }
            currentSpeed = speed;
            CameraShake.Instance.shake(0.3f, 0.5f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            state = "attacking";
        }
    }
}
