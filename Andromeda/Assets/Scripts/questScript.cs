using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class questScript : MonoBehaviour
{
    //input these values
    public string questName;
    public string[] questSteps;
    public string[] stepRequirements;
    public string[] stepAmounts;
    public bool[] upgradeStep;
    public string[] rewards;
    public bool hasRewards;
    public int[] rewardAmounts;
    public int xpAmount;
    public bool completedQuest;


    //used values
    private bool isActive;
    private int currentStep = 0;
    private bool startedStep;
    private int[] currentValues;
    private string[] newStepMaterials;
    private int[] newStepAmounts;

    private GameManager manager;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            completedQuest = manager.completedQuests.Contains(questName);
        }
    }

    private void Update()
    {
        isActive = manager.activeQuest == questName;
        if (GameObject.Find("QuestBox") != null && isActive)
        {

            GameObject.Find("QuestTitle").GetComponent<TMP_Text>().text = questName;
            string questInfo = "";
            for (int i = 0; i < questSteps.Length; i++)
            {
                if (currentStep > i)
                {
                    questInfo += "■" + questSteps[i] + "\n";
                }
                else
                {
                    questInfo += "□" + questSteps[i] + "\n";
                }
            }
            GameObject.Find("QuestInfo").GetComponent<TMP_Text>().text = questInfo;

            string rewardsInfo = "";

            rewardsInfo += "+" + xpAmount + " XP\n";
            for(int i = 0; i < rewards.Length; i++)
            {
                rewardsInfo += "+" + rewardAmounts[i] + " " + rewards[i] + "\n";
            }
            GameObject.Find("RewardsInfo").GetComponent<TMP_Text>().text = rewardsInfo;
        }
        if (isActive && currentStep < stepRequirements.Length && !completedQuest && manager.saveFile != "")
        {
            if(upgradeStep[currentStep])
            {
                if (!startedStep)
                {
                    startedStep = true;
                }
                else if (manager.getUpgradeLevel(stepRequirements[currentStep]) >= int.Parse(stepAmounts[currentStep]))
                {
                    startedStep = false;
                    currentStep++;
                }
            }
            else
            {
                if(!startedStep)
                {
                    newStepMaterials = stepRequirements[currentStep].Split(",");
                    newStepAmounts = new int[newStepMaterials.Length];
                    string[] i = stepAmounts[currentStep].Split(",");
                    for (int j = 0; j < newStepMaterials.Length; j++)
                    {
                        newStepAmounts[j] = int.Parse(i[j]);
                    }

                    currentValues = manager.getItemAmounts(newStepMaterials);
                    startedStep = true;
                }
                else
                {
                    for(int j = 0; j < currentValues.Length; j++)
                    {
                        if(currentValues[j] + newStepAmounts[j] > manager.getItemAmount(newStepMaterials[j]))
                        {
                            break;
                        }
                        else
                        {
                            if(j == currentValues.Length - 1)
                            {
                                currentStep++;
                                startedStep = false;
                            }
                        }
                    }
                }
            }
        }
        else if(isActive && GameObject.Find("GameManager") != null && currentStep == stepAmounts.Length && !completedQuest)
        {
            completedQuest = true;
            if(hasRewards)
                manager.addItems(rewards, rewardAmounts);
            manager.astroXP += xpAmount;
            manager.completedQuests.Add(questName);
            StartCoroutine(CompleteQuestAnimation());
        }
    }

    IEnumerator CompleteQuestAnimation()
    {
        GameObject.Find("QuestBox").GetComponent<Animator>().SetTrigger("QuestComplete");
        yield return new WaitForSecondsRealtime(5);
        manager.activeQuest = "";
    }

}
