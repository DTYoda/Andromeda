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
    public TMP_Text rewardText;
    public RawImage[] steps;

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
            steps[i].color = Color.white;
        }
    }

    private void Update()
    {
        if(manager != null)
        {
            amountText.text = currentAmount + "/" + neededAmounts[currentLevel];
            rewardText.text = xp[currentLevel] + " XP";
            slider.value = (float)currentAmount / (float)neededAmounts[currentLevel];

            if (isMaterial)
            {
                currentAmount = manager.getTotalItemAmount(materialName);
            }
        }


        if(currentAmount >= neededAmounts[currentLevel] && currentLevel < maxLevel)
        {
            steps[currentLevel].color = Color.white;
            currentLevel++;
        }
    }
}
