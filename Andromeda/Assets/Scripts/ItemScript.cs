using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    public GameObject textObject;
    private GameObject currentTextObj = null;
    public bool isLooking;
    private PlayerArmController arm;

    private void Start()
    {
        arm = GameObject.Find("Player").GetComponent<PlayerArmController>();
    }

    private void Update()
    {
        if (arm.currentPickUp == this.gameObject)
        {
            if(currentTextObj == null)
                currentTextObj = Instantiate(textObject);
            currentTextObj.SetActive(true);
            currentTextObj.transform.position = this.transform.position + 0.5f * (this.transform.position - GameObject.Find("planet").transform.position).normalized;
            currentTextObj.transform.eulerAngles = Camera.main.transform.eulerAngles;
            currentTextObj.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = itemName;
        }
        else if (currentTextObj != null)
        {
            Destroy(currentTextObj);
            currentTextObj = null;
        }
        
    }

    public void OnDestroy()
    {
        Destroy(currentTextObj);
    }
}
