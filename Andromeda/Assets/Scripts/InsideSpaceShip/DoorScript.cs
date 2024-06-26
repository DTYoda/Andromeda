using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            GetComponent<Animator>().SetBool("isOpen", true);
            GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            GetComponent<Animator>().SetBool("isOpen", false);
            GetComponent<AudioSource>().Play();
        }
    }
}
