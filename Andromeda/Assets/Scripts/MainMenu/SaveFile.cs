using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveFile : MonoBehaviour
{
    [SerializeField] private ConfirmationWindow confirmWindow;

    public string fileName;
    public TMP_Text text;
    string path;
    public bool DeleteMode;

    private GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();

        path = Application.persistentDataPath + "/" + fileName + ".data";
        if(File.Exists(path) && !DeleteMode)
        {
            text.text = "Continue";
        }
        else
        {
            text.text = "New Game";
        }
    }

    private void OnMouseUpAsButton()
    {
        if(Time.timeScale == 1)
        {
            if (DeleteMode)
            {
                Time.timeScale = 0;
                confirmWindow.gameObject.SetActive(true);
                confirmWindow.messageText.text = "Delete save file?";
                confirmWindow.yesButton.onClick.AddListener(Delete);
                confirmWindow.noButton.onClick.AddListener(Exit);
                transform.localScale /= 1.2f;
            }
            else
            {
                manager.saveFile = fileName;
                manager.StartGame();
            }

            GameObject.Find("ClickSound").GetComponent<AudioSource>().Play();
        }
    }

    private void OnMouseEnter()
    {
        if (Time.timeScale == 1)
        {
            if (!DeleteMode)
                transform.parent.localScale *= 1.2f;
            else
                transform.localScale *= 1.2f;
        }
    }
    private void OnMouseExit()
    {
        if(Time.timeScale == 1)
        {
            if (!DeleteMode)
                transform.parent.localScale /= 1.2f;
            else
                transform.localScale /= 1.2f;
        }
    }

    private void Delete()
    {
        File.Delete(path);
        text.text = "New Game";
        confirmWindow.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void Exit()
    {
        confirmWindow.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
