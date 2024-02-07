using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    //saving data
    [System.NonSerialized] public float health = 100;
    [System.NonSerialized] public string destroyedObjects = "";
    [System.NonSerialized] public Vector3 playerLocation = Vector3.zero;
    [System.NonSerialized] public GameObject player;
    [System.NonSerialized] public int currentPlanet = 1;
    [System.NonSerialized] public bool isInSpaceShip = false;
    [System.NonSerialized] public bool[] unlockedPlanets = { true, false };
    [System.NonSerialized] public string saveFile;

    //ArmUpgrades
    [System.NonSerialized] public float armFuel = 20;
    [System.NonSerialized] public float maxArmFuel = 20;

    //HelmetUpgrades
    [System.NonSerialized] public float oxygen = 300;
    [System.NonSerialized] public float totalOxygen = 300;

    public bool autoSaves = true;
    private float autoSaveTimer = 10;


    public List<string> materialNames =  new List<string>() { "mushroom", "shadow mushroom", "wood", "shadow wood", "plum wood", "rock", "amethyst", "topaz", "saphire" };
    public int[] materialAmounts = {                               0,             0,           0,          0,            0,         0,        0,        0,        0 };

    public static GameManager manager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        manager = this;
    }

    private void Start()
    {

        
    }

    private void Update()
    {
        if(autoSaveTimer <= 0)
        {
            SaveData();
            autoSaveTimer = 10;
        }
        autoSaveTimer -= Time.deltaTime;
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.Load(saveFile);

        health = data.health;
        destroyedObjects = data.destroyedObjets;
        playerLocation = new Vector3(data.location[0], data.location[1], data.location[2]);
        currentPlanet = data.currentPlanet;
        unlockedPlanets = data.unlockedPlanets;
        armFuel = data.armFuel;
        maxArmFuel = data.maxArmFuel;

        oxygen = data.oxygen;
        totalOxygen = data.totalOxygen;

        isInSpaceShip = data.isInSpaceShip;

        materialAmounts = data.materialAmounts;
    }

    public void SaveData()
    {
        SaveSystem.Save(GetComponent<GameManager>(), saveFile);
    }

    public void StartGame()
    {
        StartCoroutine(Begin());
    }

    public IEnumerator Begin()
    {
        CameraShake.Instance.shake(5, 8);
        string path = Application.persistentDataPath + "/" + saveFile + ".data";
        if(File.Exists(path))
        {
            LoadData();
        }
        else
        {
            SaveData();
        }

        GameObject.Find("1.5").GetComponent<Animator>().SetTrigger("start");

        yield return new WaitForSeconds(5);
        GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(3);
        if(isInSpaceShip)
            SceneManager.LoadScene("SpaceShip");
        else
        {
            SceneManager.LoadScene(currentPlanet);
        }
    }

    public void addItem(string item, int amount)
    {
        materialAmounts[materialNames.IndexOf(item)] += amount;
    }

    public int getItemAmount(string item)
    {
        return materialAmounts[materialNames.IndexOf(item)];
    }

    

}
