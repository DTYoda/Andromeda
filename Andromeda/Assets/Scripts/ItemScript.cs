using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    private GameObject textObject;
    public bool isLooking;
    private PlayerArmController arm;

    private void Start()
    {
        textObject = transform.GetChild(0).gameObject;
        arm = GameObject.Find("Player").GetComponent<PlayerArmController>();
    }

    private void Update()
    {
        textObject.transform.position = this.transform.position + 0.5f * (this.transform.position - GameObject.Find("planet").transform.position).normalized;
        textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

        textObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = itemName;

        textObject.SetActive((arm.currentPickUp == this.gameObject));
    }
}
