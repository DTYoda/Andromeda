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
    public int maxLevel = 5;
    public string[] rewards;

    //instantiated variables
    public UpgradeScript upgrader;
    private TMP_Text text = null;
    private GameManager manager;
    private int upgradeLevel = 0;

    //error and confirmation windows;
    [SerializeField] private ConfirmationWindow confirmWindow;
    [SerializeField] private ErrorWindow errorWindow;
 
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
            for (int i = 1; i <= maxLevel; i++)
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
                text.text += "\n\tRequires Atronaut Level " + (upgradeLevel + 1);
                text.text += "\n\t" + rewards[upgradeLevel];
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
        confirmWindow.gameObject.SetActive(true);
        confirmWindow.yesButton.onClick.RemoveAllListeners();
        confirmWindow.noButton.onClick.RemoveAllListeners();
        confirmWindow.yesButton.onClick.AddListener(yesUpgrade);
        confirmWindow.noButton.onClick.AddListener(noUpgrade);
        confirmWindow.messageText.text = "Upgrade " + upgradeName + "?";
    }

    private void yesUpgrade()
    {
        if (upgradeLevel < upgradeItems.Length && upgradeLevel <= manager.astroLevel)
        {
            if (upgrader.Upgrade(upgradeItems[upgradeLevel].Split(","), upgradeItemsAmounts[upgradeLevel].Split(","), upgradeName, upgradeLevelAmounts[upgradeLevel]))
            {
                upgradeLevel++;
                if (GameObject.Find("GameManager") != null)
                {
                    manager.addUpgrade(upgradeName);
                    GameObject.Find("UpgradeSound").GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                errorWindow.gameObject.SetActive(true);
                errorWindow.messageText.text = "You do not have enough resources for this upgrade";
                errorWindow.exitButton.onClick.RemoveAllListeners();
                errorWindow.exitButton.onClick.AddListener(noUpgrade);
            }
        }
        else
        {
            errorWindow.gameObject.SetActive(true);
            errorWindow.messageText.text = "You must be level " + (upgradeLevel + 1) + " for this upgrade";
            errorWindow.exitButton.onClick.RemoveAllListeners();
            errorWindow.exitButton.onClick.AddListener(noUpgrade);
        }
        confirmWindow.gameObject.SetActive(false);
    }
    private void noUpgrade()
    {
        errorWindow.messageText.text = "";
        confirmWindow.gameObject.SetActive(false);
        errorWindow.gameObject.SetActive(false);
    }
}
