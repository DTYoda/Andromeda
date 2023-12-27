using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //saving data
    [System.NonSerialized] public float health = 100;
    [System.NonSerialized] public string[] destroyedObjects = { };
    [System.NonSerialized] public Vector3 playerLocation = Vector3.zero;
    [System.NonSerialized] public GameObject player;
    [System.NonSerialized] public int currentPlanet = 1;
    [System.NonSerialized] public bool[] unlockedPlanets = { true, false };
    [System.NonSerialized] public string saveFile;

    public bool autoSaves;


    public Dictionary<string, int> materials = new Dictionary<string, int>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        materials.Add("wood", 0);
        materials.Add("hard wood", 0);
        materials.Add("plum wood", 0);
        materials.Add("rock", 0);
        materials.Add("amethyst", 0);
        materials.Add("topaz", 0);
        materials.Add("saphire", 0);
        materials.Add("mushroom", 0);

        if(autoSaves)
        {
            StartCoroutine(AutoSave());
        }
    }

    private void Update()
    {
        
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.Load(saveFile);

        health = data.health;
        destroyedObjects = data.destroyedObjets;
        playerLocation = new Vector3(data.location[0], data.location[1], data.location[2]);
        currentPlanet = data.currentPlanet;
        unlockedPlanets = data.unlockedPlanets;
    }

    public void SaveData()
    {
        SaveSystem.Save(GetComponent<GameManager>(), saveFile);
    }

    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(10);
        SaveData();
        if(autoSaves)
            StartCoroutine(AutoSave());
    }

    public void StartGame()
    {
        StartCoroutine(Begin());
    }

    public IEnumerator Begin()
    {
        string path = Application.persistentDataPath + "/" + saveFile + ".data";
        if(File.Exists(path))
        {
            LoadData();
        }
        else
        {
            SaveData();
            Debug.Log("creatingNew");
        }

        GameObject.Find("1.5").GetComponent<Animator>().SetTrigger("start");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(currentPlanet);
    }


}
