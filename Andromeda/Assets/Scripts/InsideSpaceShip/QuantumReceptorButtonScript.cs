using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuantumReceptorButtonScript : MonoBehaviour
{
    public string questName;
    public TMP_Text title;
    public TMP_Text Description;
    public TMP_Text Planet;
    public Image starImage;
    public questScript quest;

    private GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        title.text = ">>" + quest.questSender + "\n" + quest.questName;
        Description.text = quest.questDescription.Substring(0, 45) + "...";
        Planet.text = quest.questPlanet;
    }

    public void Click()
    {
        GetComponentInParent<QuantumReceptorScript>().ChangeDescriptionText(quest);
    }

    private void Update()
    {
        starImage.color = (manager.completedQuests.Contains(quest.questName) ? Color.white : Color.black);
    }

}
