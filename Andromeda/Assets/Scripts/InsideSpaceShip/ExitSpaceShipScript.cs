using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitSpaceShipScript : MonoBehaviour
{
    public GameObject textObject;
    public bool canEnterSpaceship = true;

    public LayerMask mask;

    private void Update()
    {
        textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5, mask) && canEnterSpaceship)
        {
            textObject.SetActive(hit.transform.gameObject == this.gameObject);
            textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
            if (hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E) && GameObject.Find("GameManager") != null && canEnterSpaceship)
            {
                GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                manager.isInSpaceShip = true;
                SceneManager.LoadScene("SpaceShip");
            }
            else if(hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("SpaceShip");
            }
        }
        else
        {
            textObject.SetActive(false);
        }

    }
}
