using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalScript : MonoBehaviour
{
    public GameObject textObject;
    public LayerMask mask;

    private void Update()
    {
        textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, mask))
        {
            textObject.SetActive(hit.transform.gameObject == this.gameObject);
        }
        else
        {
            textObject.SetActive(false);
        }
            
    }
}
