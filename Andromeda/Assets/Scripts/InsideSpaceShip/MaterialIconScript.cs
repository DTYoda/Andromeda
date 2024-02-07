using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialIconScript : MonoBehaviour
{
    public string itemName;
    private GameObject info;
    private GameManager manager = null;
    // Start is called before the first frame update
    void Start()
    {
        info = transform.GetChild(1).gameObject;
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(manager != null)
        {
            transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = manager.getItemAmount(itemName).ToString();
        }
    }

    private void OnMouseEnter()
    {
        info.SetActive(true);
    }
    private void OnMouseExit()
    {
        info.SetActive(false);
    }
}
