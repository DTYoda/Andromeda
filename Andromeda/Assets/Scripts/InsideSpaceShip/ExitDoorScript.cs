using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorScript : MonoBehaviour
{

    public GameObject textObject;
    private bool isInside;

    private void Update()
    {
        if(isInside)
        {
            if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("GameManager") != null)
            {
                GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                manager.isInSpaceShip = false;
                SceneManager.LoadScene(manager.currentPlanet);
            }
            textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            textObject.SetActive(true);
            isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        textObject.SetActive(false);
        isInside = false;
    }
}
