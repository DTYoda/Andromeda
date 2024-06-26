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

    [SerializeField] private ConfirmationWindow confirmWindow;

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
        confirmWindow.gameObject.SetActive(true);
        confirmWindow.yesButton.onClick.RemoveAllListeners();
        confirmWindow.noButton.onClick.RemoveAllListeners();
        confirmWindow.yesButton.onClick.AddListener(YesStartQuest);
        confirmWindow.noButton.onClick.AddListener(NoStartQuest);
        confirmWindow.messageText.text = "Start Quest?";
    }

    public void YesStartQuest()
    {
        manager.activeQuest = selectedQuest.questName;
        manager.activeQuestStep = 0;
        startButtonText.text = "Ongoing Quest!";
        confirmWindow.gameObject.SetActive(false);
    }

    public void NoStartQuest()
    {
        confirmWindow.gameObject.SetActive(false);
    }
}