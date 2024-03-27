using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingIconAmount : MonoBehaviour
{
    private float WASDAmount = 2;
    private float spaceAmount = 1;
    private float HAmount = 1;
    private float FAmount = 1;
    private float IAmount = 1;

    public GameObject wasdIcon;
    public GameObject spaceIcon;
    public GameObject FIcon;
    public GameObject HIcon;
    public GameObject IIcon;

    private GameManager manager;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            spaceAmount--;
        }

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            WASDAmount -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            FAmount--;
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            HAmount--;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            IAmount--;
        }

        if (WASDAmount <= 0 || manager.completedTutorial || GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship)
        {
            wasdIcon.SetActive(false);
        }
        if(spaceAmount <= 0 || manager.completedTutorial || GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship)
        {
            spaceIcon.SetActive(false);
        }
        if (FAmount <= 0 || manager.completedTutorial || GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship)
        {
            FIcon.SetActive(false);
        }
        if (HAmount <= 0|| manager.completedTutorial || GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship)
        {
            HIcon.SetActive(false);
        }
        if (IAmount <= 0 || manager.completedTutorial || GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship)
        {
            IIcon.SetActive(false);
        }
    }
}
