using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveFile : MonoBehaviour
{
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
        if(DeleteMode)
        {
            File.Delete(path);
            text.text = "New Game";
        }
        else
        {
            manager.saveFile = fileName;
            manager.StartGame();
        }

    }

    private void OnMouseEnter()
    {
        transform.parent.localScale *= 1.2f;
    }
    private void OnMouseExit()
    {
        transform.parent.localScale /= 1.2f;
    }
}
