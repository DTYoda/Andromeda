using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuantumReceptorScript : MonoBehaviour
{
    public TMP_Text missionDescriptionText;
    public GameObject qrButton;
    public TMP_Text startButtonText;
    public Button startButton;

    private GameManager manager;
    private questScript selectedQuest;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i = 0; i < manager.QRQuests.Count; i++)
        {
            Instantiate(qrButton, this.transform).GetComponent<QuantumReceptorButtonScript>().quest = manager.transform.Find("QuestManager").Find(manager.QRQuests[i]).gameObject.GetComponent<questScript>();
        }
    }

    private void Update()
    {
        
    }

    public void ChangeDescriptionText(questScript quest)
    {
        missionDescriptionText.text = quest.name + "\n\n" + quest.questDescription + "\n\n-" + quest.questSender;
        selectedQuest = quest;

        if (manager.completedQuests.Contains(quest.name))
        {
            startButtonText.text = "Quest Complete!";
            startButton.interactable = false;
        }
        else if (manager.activeQuest == quest.name)
        {
            startButtonText.text = "Ongoing Quest!";
            startButton.interactable = false;
        }
        else
        {
            startButtonText.text = "Start Quest";
            startButton.interactable = true;
        }
    }

    public void StartQuest()
    {
        manager.activeQuest = selectedQuest.questName;
        manager.activeQuestStep = 0;
    }
}