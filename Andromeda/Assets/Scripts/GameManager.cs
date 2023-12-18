using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float health = 100;

    public Dictionary<string, int> materials = new Dictionary<string, int>();

    //Current Object Hologram and text
    public GameObject hologramParent;
    public GameObject currentObject;
    public GameObject objHologram;
    public TMP_Text hologramText;
    public GameObject objHealthBar;

    private void Start()
    {
        materials.Add("wood", 0);
        materials.Add("hard wood", 0);
        materials.Add("plum wood", 0);
        materials.Add("rock", 0);
        materials.Add("amethyst", 0);
        materials.Add("topaz", 0);
        materials.Add("saphire", 0);
        materials.Add("mushroom", 0);
    }

    private void Update()
    {
        LookingAt();
    }

    private void LookingAt()
    {
        if (currentObject != null)
        {
            MineableObject obj = currentObject.GetComponent<MineableObject>();
            hologramText.text = "Name: " + obj.objName + "\nHardness: " + obj.hardness + "\nDrop: " + obj.itemDrop;
            objHologram = hologramParent.transform.Find(obj.itemDrop).gameObject;
            objHologram.SetActive(true);
            objHealthBar.SetActive(true);
            objHologram.transform.localEulerAngles += Vector3.up * Time.deltaTime * 10;
            objHealthBar.transform.GetChild(0).localScale = new Vector3(obj.currentHealth / obj.totalHealth, 1, 1);
        }
        else
        {
            if(objHologram != null)
            {
                objHologram.SetActive(false);
            }
            hologramText.text = "";
            objHealthBar.SetActive(false);
        }
    }
}
