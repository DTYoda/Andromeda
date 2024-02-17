using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingScript : MonoBehaviour
{
    public GameObject textObject;
    public LayerMask mask;

    public GameObject craftingUI;

    private void Update()
    {
        textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, mask))
        {
            textObject.SetActive(hit.transform.gameObject == this.gameObject);
            if (hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                craftingUI.SetActive(true);
            }
        }
        else
        {
            textObject.SetActive(false);
        }
            
    }

    public void exitCrafting()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        craftingUI.SetActive(false);
    }
}
