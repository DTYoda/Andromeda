using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenQRScript : MonoBehaviour
{
    public GameObject textObject;
    public LayerMask mask;

    public GameObject qrUI;
    private GameManager manager = null;

    private bool isActive;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        isActive = (manager != null && manager.getUpgradeLevel("Quantum Receptor") >= 1);

        if(isActive)
        {
            GetComponent<MeshRenderer>().enabled = true;
            textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, mask))
            {
                textObject.SetActive(hit.transform.gameObject == this.gameObject);
                if (hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    qrUI.SetActive(true);
                }
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

       

    }

    public void exitQR()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        qrUI.SetActive(false);
    }
}
