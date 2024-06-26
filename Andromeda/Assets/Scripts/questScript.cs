using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class questScript : MonoBehaviour
{
    //Description Variables
    public string questName;
    public string questDescription;
    public string questSender;
    public string questPlanet;

    //Quest Information
    public string[] questSteps;
    public string[] stepRequirements;
    public string[] stepAmounts;
    public bool[] upgradeStep;
    public bool[] craftingStep;
    public string[] rewards;
    public bool hasRewards;
    public int[] rewardAmounts;
    public int xpAmount;
    public bool completedQuest;


    //helper variables
    private bool isActive;
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
        if (GameObject.Find("QuestBox") != null && isActive && !completedQuest)
        {

            GameObject.Find("QuestTitle").GetComponent<TMP_Text>().text = questName;
            string questInfo = "";
            for (int i = 0; i < questSteps.Length; i++)
            {
                if (manager.activeQuestStep > i)
                {
                    questInfo += "■" + questSteps[i] + "\n";
                }
                else
                {
                    questInfo += "□" + questSteps[i] + "\n";
                }
            }
            GameObject.Find("QuestInfo").GetComponent<TMP_Text>().text = questInfo;
        }
        if (isActive && manager.activeQuestStep < stepRequirements.Length && !completedQuest && manager.saveFile != "")
        {
            if(upgradeStep[manager.activeQuestStep])
            {
                if (!startedStep)
                {
                    startedStep = true;
                }
                else if (manager.getUpgradeLevel(stepRequirements[manager.activeQuestStep]) >= int.Parse(stepAmounts[manager.activeQuestStep]))
                {
                    startedStep = false;
                    manager.activeQuestStep++;
                    manager.astroXP++;
                    GetComponent<AudioSource>().Play();
                }
            }

            else if(craftingStep[manager.activeQuestStep])
            {
                if(!startedStep)
                {
                    currentValues = new int[] { manager.getCraftAmount(stepRequirements[manager.activeQuestStep]) };
                    startedStep = true;
                }
                else
                {
                    if(manager.getCraftAmount(stepRequirements[manager.activeQuestStep]) == currentValues[0] + 1)
                    {
                        manager.activeQuestStep++;
                        startedStep = false;
                        manager.astroXP++;
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
            else
            {
                if(!startedStep)
                {
                    newStepMaterials = stepRequirements[manager.activeQuestStep].Split(",");
                    newStepAmounts = new int[newStepMaterials.Length];
                    string[] i = stepAmounts[manager.activeQuestStep].Split(",");
                    for (int j = 0; j < newStepMaterials.Length; j++)
                    {
                        newStepAmounts[j] = int.Parse(i[j]);
                    }

                    startedStep = true;
                }
                else
                {
                    for(int j = 0; j < newStepAmounts.Length; j++)
                    {
                        if(newStepAmounts[j] > manager.getItemAmount(newStepMaterials[j]))
                        {
                            break;
                        }
                        else
                        {
                            if(j == newStepAmounts.Length - 1)
                            {
                                manager.activeQuestStep++;
                                startedStep = false;
                                manager.astroXP++;
                                GetComponentInParent<AudioSource>().Play();
                            }
                        }
                    }
                }
            }
        }
        else if(isActive && GameObject.Find("GameManager") != null && manager.activeQuestStep == stepAmounts.Length && !completedQuest)
        {
            completedQuest = true;
            manager.activeQuestStep = 0;
            if (hasRewards)
                manager.addItems(rewards, rewardAmounts);
            manager.astroXP += xpAmount;
            manager.completedQuests.Add(questName);
            StartCoroutine(CompleteQuestAnimation());
        }
    }

    IEnumerator CompleteQuestAnimation()
    {
        if(GameObject.Find("QuestBox") != null)
            GameObject.Find("QuestBox").GetComponent<Animator>().SetTrigger("QuestComplete");
        yield return new WaitForSecondsRealtime(2f);
        
        //set rewards
        string rewardsInfo = "";
        rewardsInfo += "+" + xpAmount + " XP\n";
        for (int i = 0; i < rewards.Length; i++)
        {
            rewardsInfo += "+" + rewardAmounts[i] + " " + rewards[i] + "\n";
        }
        GameObject.Find("RewardsInfo").GetComponent<TMP_Text>().text = rewardsInfo;

        yield return new WaitForSecondsRealtime(3f);
        manager.completedQuests.Add(questName);
        manager.activeQuest = "";
    }

}
