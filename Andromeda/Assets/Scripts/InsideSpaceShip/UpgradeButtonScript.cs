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

    private void Start()
    {
        text = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
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

            for (int j = 0; j < upgradeItems[upgradeLevel].Split(" ").Length; j++)
            {
                text.text += "\n\t" + upgradeItemsAmounts[upgradeLevel].Split(" ")[j] + " " + upgradeItems[upgradeLevel].Split(" ")[j];
            }
        }
        
    }


    public void upgradeButton()
    {
        if(upgradeLevel < upgradeItems.Length)
        {
            if (upgrader.Upgrade(upgradeItems[upgradeLevel].Split(" "), upgradeItemsAmounts[upgradeLevel].Split(), upgradeName, upgradeLevelAmounts[upgradeLevel]))
            {
                upgradeLevel++;
            }
        }
        
    }
}
