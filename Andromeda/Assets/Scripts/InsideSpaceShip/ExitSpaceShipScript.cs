using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitSpaceShipScript : MonoBehaviour
{
    public GameObject textObject;
    private GameObject currentTextObject;
    public bool canEnterSpaceship = true;

    public LayerMask mask;

    private void Update()
    {
        

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5, mask) && canEnterSpaceship)
        {
            if(currentTextObject == null)
            {
                currentTextObject = Instantiate(textObject);
                currentTextObject.transform.localScale *= 3;
            }
            currentTextObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
            currentTextObject.SetActive(hit.transform.gameObject == this.gameObject);
            currentTextObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
            currentTextObject.transform.position = this.transform.position + 2f * (this.transform.position - GameObject.Find("planet").transform.position).normalized;
            currentTextObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "spaceship";
            currentTextObject.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "press \"E\" to enter";
            if (hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E) && GameObject.Find("GameManager") != null && canEnterSpaceship)
            {
                GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                manager.isInSpaceShip = true;
                SceneManager.LoadScene("SpaceShip");
                Destroy(currentTextObject);
            }
            else if(hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("SpaceShip");
                Destroy(currentTextObject);
            }
        }
        else if(currentTextObject != null)
        {
            Destroy(currentTextObject);
            currentTextObject = null;
        }

    }
}
