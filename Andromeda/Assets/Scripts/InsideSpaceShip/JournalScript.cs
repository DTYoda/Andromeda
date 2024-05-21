using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalScript : MonoBehaviour
{
    public GameObject textObject;
    public GameObject UI;
    public LayerMask mask;

    private bool isActive = false;
    private GameManager manager = null;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void Update()
    {

        if(manager != null)
        {
            isActive = manager.getUpgradeLevel("Journal") >= 1;
        }

        if(isActive)
        {
            GetComponent<MeshRenderer>().enabled = true;

            textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, mask))
            {
                textObject.SetActive(hit.transform.gameObject == this.gameObject);
            }
            else
            {
                textObject.SetActive(false);
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            textObject.SetActive(false);
        }
        
        if(isActive && textObject.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            UI.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ExitJournal()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(false);
    }
}
