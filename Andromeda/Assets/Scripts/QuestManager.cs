using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    private GameManager manager;
    private GameObject questBox;
    private GameObject questRewards;
    private void Update()
    {
        if (questBox != null)
        {
            questBox.SetActive(manager.activeQuest != "");
        }
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("QuestBox") != null)
        {
            questBox = GameObject.Find("QuestBox");
            questBox.SetActive(manager.activeQuest != "");
        }
    }
}
