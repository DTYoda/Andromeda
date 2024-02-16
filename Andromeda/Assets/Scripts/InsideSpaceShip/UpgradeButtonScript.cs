using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButtonScript : MonoBehaviour
{
    //Input these variables
    public string upgradeName;
    public string[] upgradeItems;
    public string[] upgradeItemsAmounts;
    public float[] upgradeLevelAmounts;
    private int upgradeLevel = 0;

    //instantiated variables
    public UpgradeScript upgrader;
    private TMP_Text text = null;
    private GameManager manager;

    private void Start()
    {
        upgradeName = this.name;

        text = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            upgradeLevel = manager.getUpgradeLevel(upgradeName);
        }
    }

    private void Update()
    {
        if(text  != null)
        {
            text.text = upgradeName;
            for (int i = 1; i <= 5; i++)
            {
                if (i <= upgradeLevel)
                {
                    text.text += " ■";
                }
                else
                {
                    text.text += " □";
                }
            }

            if(upgradeLevel < upgradeItems.Length)
            {
                for (int j = 0; j < upgradeItems[upgradeLevel].Split(",").Length; j++)
                {
                    text.text += "\n\t" + upgradeItemsAmounts[upgradeLevel].Split(",")[j] + " " + upgradeItems[upgradeLevel].Split(",")[j];
                }
            }
            else
            {
                text.text += "\n\tMax Level";
            }
            
        }
        
    }


    public void upgradeButton()
    {
        if(upgradeLevel < upgradeItems.Length)
        {
            if (upgrader.Upgrade(upgradeItems[upgradeLevel].Split(","), upgradeItemsAmounts[upgradeLevel].Split(","), upgradeName, upgradeLevelAmounts[upgradeLevel]))
            {
                upgradeLevel++;
                if (GameObject.Find("GameManager") != null)
                {
                    manager.addUpgrade(upgradeName);
                }
            }
        }
        
    }
}
