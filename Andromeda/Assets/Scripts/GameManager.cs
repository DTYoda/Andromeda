using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //non saved data
    [System.NonSerialized] public string saveFile = "";
    [System.NonSerialized] public GameObject player;
    public bool autoSaves = true;
    private float autoSaveTimer = 10;

    //saving data
    [System.NonSerialized] public float health = 100;
    [System.NonSerialized] public string destroyedObjects = "";
    [System.NonSerialized] public Vector3 playerLocation = Vector3.zero;
    [System.NonSerialized] public int currentPlanet = 1;
    [System.NonSerialized] public bool isInSpaceShip = false;
    [System.NonSerialized] public bool[] unlockedPlanets = { true, false };

    //ArmUpgrades
    [System.NonSerialized] public float armFuel = 20;
    [System.NonSerialized] public float maxArmFuel = 20;
    [System.NonSerialized] public float armDamage = 20;
    [System.NonSerialized] public float armStrength = 1;

    //HelmetUpgrades
    [System.NonSerialized] public float oxygen = 300;
    [System.NonSerialized] public float totalOxygen = 300;

    //Boot Upgrades
    [System.NonSerialized] public float jumpHeight = 5;
    [System.NonSerialized] public float walkSpeed = 5;
    [System.NonSerialized] public float multiJumpAmount = 0;
    [System.NonSerialized] public bool canMultiJump = false;

    //Armor Upgrades
    [System.NonSerialized] public float maxHealth = 100;
    [System.NonSerialized] public float healthRegen = 1;
    [System.NonSerialized] public float defense = 0;


    //material and upgrade data
    public List<string> materialNames =  new List<string>() { "mushroom", "shadow mushroom", "wood", "shadow wood", "plum wood", "rock", "amethyst", "topaz", "saphire" };
    public int[] materialAmounts = {                               0,             0,           0,          0,            0,         0,        0,        0,        0 };

    public List<string> upgradeNames = new List<string>() { "Max Fuel", "Mining Strength", "Damage", "Jump Height", "Walking Speed",   "Multi-Jump", "Max Health", "Health Regen", "Defense"};
    public int[] upgradeLevels = {                               0,                 0,          0,           0,             0,              0,              0,          0,              0 };


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

        //saving data
        health = data.health;
        currentPlanet = data.currentPlanet;
        unlockedPlanets = data.unlockedPlanets;
        destroyedObjects = data.destroyedObjects;
        isInSpaceShip = data.isInSpaceShip;

        playerLocation = new Vector3(data.playerLocation[0], data.playerLocation[1], data.playerLocation[2]);

        //armupgrades
        armFuel = data.armFuel;
        maxArmFuel = data.maxArmFuel;
        armDamage = data.armDamage;
        armStrength = data.armStrength;

        //helmet upgrades
        oxygen = data.oxygen;
        totalOxygen = data.totalOxygen;

        //boot upgrades
        jumpHeight = data.jumpHeight;
        walkSpeed = data.walkSpeed;
        multiJumpAmount = data.multiJumpAmount;
        canMultiJump = data.canMultiJump;

        //armor upgrades
        maxHealth = data.maxHealth;
        healthRegen = data.healthRegen;
        defense = data.defense;

        //material and upgrade data
        materialAmounts = data.materialAmounts;
        upgradeLevels = data.upgradeLevels;
    }

    public void SaveData()
    {
        if(saveFile != "")
            SaveSystem.Save(this.gameObject.GetComponent<GameManager>(), saveFile);
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

    public bool addItem(string item, int amount)
    {
        if (materialAmounts[materialNames.IndexOf(item)] + amount >= 0)
        {
            materialAmounts[materialNames.IndexOf(item)] += amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool addItems(string[] items, int[] amounts)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (materialAmounts[materialNames.IndexOf(items[i])] + amounts[i] < 0)
            {
                return false;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            materialAmounts[materialNames.IndexOf(items[i])] += amounts[i];
        }
        return true;
    }

    public int getItemAmount(string item)
    {
        return materialAmounts[materialNames.IndexOf(item)];
    }


    public void addUpgrade(string upgrade)
    {
        if(upgradeNames.IndexOf(upgrade) != -1)
        {
            upgradeLevels[upgradeNames.IndexOf(upgrade)]++;
        }
    }

    public int getUpgradeLevel(string upgrade)
    {
        if (upgradeNames.IndexOf(upgrade) != -1)
        {
            return upgradeLevels[upgradeNames.IndexOf(upgrade)];
        }
        else
        {
            return 0;
        }
    }



}
