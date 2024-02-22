using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private GameManager manager;
    private GameObject questBox;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        
    }

    private void Update()
    {
        if(GameObject.Find("QuestBox") != null)
        {
            questBox = GameObject.Find("QuestBox");
            questBox.SetActive(manager.activeQuest != "");
        }  
    }
}
