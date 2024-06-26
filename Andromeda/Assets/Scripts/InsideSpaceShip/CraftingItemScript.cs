using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemScript : MonoBehaviour
{

    private GameManager manager;

    //confirm and error windows
    [SerializeField] private ConfirmationWindow confirmWindow;
    [SerializeField] private ErrorWindow errorWindow;

    //change these variables
    public string craftItem;
    public int craftAmount;

    //item1
    public int item1Amount;
    public string item1;

    //item2
    public int item2Amount;
    public string item2;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }


    //color changing functions
    public void ChangeColorLight(Image image)
    {
        image.color = new Color(0.6588235f, 0.9490196f, 0.9843137f, 1f);
    }
    public void ChangeColorDark(Image image)
    {
        image.color = new Color(0.345098f, 0.5647059f, 0.6039216f, 1f);
    }

    //Item 1 functions
    public void changeItem1(string i)
    {
        item1 = i;
    }
    public void changeItem1Amount(int i)
    {
        item1Amount = i;
    }

    //Item 2 functions
    public void changeItem2(string i)
    {
        item2 = i;
    }
    public void changeItem2Amount(int i)
    {
        item2Amount = i;
    }

    //Craft Function
    public void Craft()
    {
        confirmWindow.gameObject.SetActive(true);
        confirmWindow.yesButton.onClick.RemoveAllListeners();
        confirmWindow.noButton.onClick.RemoveAllListeners();
        confirmWindow.yesButton.onClick.AddListener(yesCraft);
        confirmWindow.noButton.onClick.AddListener(noCraft);
        confirmWindow.messageText.text = "Craft " + craftItem + "?";
    }

    private void yesCraft()
    {
        string[] items = { item1, item2 };
        int[] itemAmounts = { item1Amount, item2Amount };
        if (GameObject.Find("GameManager") != null)
        {
            if (manager.addItems(items, itemAmounts))
            {
                switch (craftItem)
                {
                    case ("Gauntlet Fuel"):
                        manager.armFuel += craftAmount;
                        manager.addCraft(craftItem);
                        break;
                    case ("oxygen"):
                        manager.oxygen += craftAmount;
                        manager.addCraft(craftItem);
                        break;
                }
                GameObject.Find("CraftingSound").GetComponent<AudioSource>().Play();
            }
            else
            {
                errorWindow.gameObject.SetActive(true);
                errorWindow.messageText.text = "You do not have the selected resources to craft this.";
                errorWindow.exitButton.onClick.RemoveAllListeners();
                errorWindow.exitButton.onClick.AddListener(noCraft);
            }
        }
        confirmWindow.gameObject.SetActive(false);
    }

    private void noCraft()
    {
        confirmWindow.gameObject.SetActive(false);
        errorWindow.gameObject.SetActive(false);
    }


}
