using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalEntryScript : MonoBehaviour
{
    public int currentLevel;
    public int maxLevel;

    public int[] neededAmounts;
    public int currentAmount;

    public string[] xp;

    public Slider slider;
    public TMP_Text amountText;
    public Toggle[] steps;

    public bool isMaterial;
    public string materialName;

    public GameManager manager = null;

    private void Awake()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        for(int i = 0; i < currentLevel; i++)
        {
            steps[i].isOn = true;
            steps[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(manager != null)
        {
            amountText.text = currentAmount + "/" + neededAmounts[currentLevel];
            slider.value = (float)currentAmount / (float)neededAmounts[currentLevel];

            if (isMaterial)
            {
                currentAmount = manager.getTotalItemAmount(materialName);
            }
        }


        if(currentAmount >= neededAmounts[currentLevel] && currentLevel < maxLevel)
        {
            steps[currentLevel].isOn = true;
            steps[currentLevel].transform.GetChild(1).gameObject.SetActive(false);
            currentLevel++;
        }
    }
}
