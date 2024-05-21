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
    [System.NonSerialized] public bool autoSaves = true;
    [System.NonSerialized] private float autoSaveTimer = 10;

    //saving data
    [System.NonSerialized] public float health = 100;
    [System.NonSerialized] public Vector3 playerLocation = Vector3.zero;
    [System.NonSerialized] public int currentPlanet = 1;
    [System.NonSerialized] public bool isInSpaceShip = false;
    [System.NonSerialized] public bool[] unlockedPlanets = { true, false };
    [System.NonSerialized] public bool completedTutorial = false;


    //currently destroyed objects
    [System.NonSerialized] public List<string> destroyed = new List<string>();

    //ArmUpgrades
    [System.NonSerialized] public float armFuel = 60;
    [System.NonSerialized] public float maxArmFuel = 60;
    [System.NonSerialized] public float armDamage = 10;
    [System.NonSerialized] public float armStrength = 0;

    //HelmetUpgrades
    [System.NonSerialized] public float oxygen = 400;
    [System.NonSerialized] public float totalOxygen = 400;

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
    [System.NonSerialized] public List<string> materialNames =  new List<string>() { "mushroom", "shadow mushroom", "wood", "shadow wood", "plum wood", "rock", "amethyst", "topaz", "saphire" };
    [System.NonSerialized] public int[] materialAmounts = {                               0,             0,           0,          0,            0,         0,        0,        0,        0 };

    [System.NonSerialized] public List<string> upgradeNames = new List<string>() { "Max Fuel", "Mining Strength", "Damage", "Jump Height", "Walking Speed",   "Multi-Jump", "Max Health", "Health Regen", "Defense", "Jet Force", "Jet Fuel", "Upgrade Room", "Crafting Bench", "Journal", "Quantum Receptor" };
    [System.NonSerialized] public int[] upgradeLevels = {                               0,                 0,          0,           0,             0,              0,              0,          0,              0,         0,           0,            0,                0,          0,              0 };

    [System.NonSerialized] public List<string> craftNames = new List<string>() { "oxygen", "Gauntlet Fuel" };
    [System.NonSerialized] public int[] timesCrafted = { 0, 0 };

    //quest data
    [System.NonSerialized] public List<string> completedQuests = new List<string>();
    [System.NonSerialized] public List<string> QRQuests = new List<string>() { "Surviving 101" };
    [System.NonSerialized] public string activeQuest = "Repair Quantum Receptor";
    [System.NonSerialized] public int activeQuestStep = 0;
    [System.NonSerialized] public float astroXP = 0;
    [System.NonSerialized] public string[] atroLevelNames = { "civilian", "engineer", "pilot", "commandar", "specialist", "cosmonaut" };
    [System.NonSerialized] public int astroLevel = 0;

    //journal data
    [System.NonSerialized] public int[] totalMaterialsCollected = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


    public static GameManager manager;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        manager = this;
    }

    private void Start()
    {

        
    }

    private bool dyingAnim = false;
    private void Update()
    {
        if(autoSaveTimer <= 0)
        {
            if(autoSaves)
                SaveData();
            autoSaveTimer = 10;
            if(Random.Range(0, 2) == 0)
            {
                PickRandomQuest();
            }
        }
        autoSaveTimer -= Time.deltaTime;

        //regeneration
        if(health < maxHealth)
        {
            health += healthRegen * Time.deltaTime;
        }
        if(armFuel < maxArmFuel)
        {
            armFuel += Time.deltaTime / 10.0f;
        }

        if(!isInSpaceShip && completedTutorial)
        {
            if(oxygen > 0)
            {
                oxygen -= Time.deltaTime;
            }
            else
            {
                health -= 3 * Time.deltaTime;
            }
        }
        else
        {
            oxygen += Time.deltaTime / 3.0f;
        }

        //setting maximums
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health < 0)
        {
            health = 0;
            if(!dyingAnim)
                StartCoroutine(Death());
        }

        if(armFuel > maxArmFuel)
        {
            armFuel = maxArmFuel;
        }
        else if(armFuel < 0)
        {
            armFuel = 0;
        }

        if(oxygen > totalOxygen)
        {
            oxygen = totalOxygen;
        }
        else if(oxygen < 0)
        {
            oxygen = 0;
        }

    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.Load(saveFile);

        //saving data
        health = data.health;
        currentPlanet = data.currentPlanet;
        unlockedPlanets = data.unlockedPlanets;
        isInSpaceShip = data.isInSpaceShip;

        playerLocation = new Vector3(data.playerLocation[0], data.playerLocation[1], data.playerLocation[2]);

        destroyed = data.destroyed;
        completedTutorial = data.completedTutorial;

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

        //quests data
        completedQuests = data.completedQuests;
        QRQuests = data.QRQuests;
        activeQuest = data.activeQuest;
        activeQuestStep = data.activeQuestStep;
        astroXP = data.astroXP;
        astroLevel = data.astroLevel;
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

    //call to start the game
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

    //Call to add an item to the player's inventory
    public bool addItem(string item, int amount)
    {
        if (materialAmounts[materialNames.IndexOf(item)] + amount >= 0)
        {
            materialAmounts[materialNames.IndexOf(item)] += amount;
            if (amount > 0)
            {
                totalMaterialsCollected[materialNames.IndexOf(item)] += amount;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //Call to add multiple items to the player's inventory
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
            if(amounts[i] > 0)
            {
                totalMaterialsCollected[materialNames.IndexOf(items[i])] += amounts[i];
            }
        }
        return true;
    }

    //Call to recieve the number of a certain item the player has
    public int getItemAmount(string item)
    {
        return materialAmounts[materialNames.IndexOf(item)];
    }

    //call to receive the number of multiple items the player has
    public int[] getItemAmounts(string[] items)
    {
        int[] amounts = new int[items.Length];

        for(int i = 0; i < amounts.Length; i++)
        {
            amounts[i] = materialAmounts[materialNames.IndexOf(items[i])];
        }
        return amounts;
    }

    //Call to recieve the number of a certain item the player has ever collected
    public int getTotalItemAmount(string item)
    {
        return totalMaterialsCollected[materialNames.IndexOf(item)];
    }

    //Call to increase the value of a player's upgrade
    public void addUpgrade(string upgrade)
    {
        if(upgradeNames.IndexOf(upgrade) != -1)
        {
            upgradeLevels[upgradeNames.IndexOf(upgrade)]++;
        }
    }

    //call to recieve the value of a player's upgrade
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

    //call to increase the times a certain craft has been crafted
    public void addCraft(string craft)
    {
        timesCrafted[craftNames.IndexOf(craft)]++;
    }

    //Call to recieve the number a certain item has been crafted
    public int getCraftAmount(string craft)
    {
        return timesCrafted[craftNames.IndexOf(craft)];
    }

    private void PickRandomQuest()
    {
        questScript quest;

        for(int i = 0; i < transform.Find("QuestManager").childCount; i++)
        {
            quest = transform.Find("QuestManager").GetChild(Random.Range(1, transform.Find("QuestManager").childCount)).gameObject.GetComponent<questScript>();
            if(!QRQuests.Contains(quest.questName))
            {
                QRQuests.Add(quest.questName);
                break;
            }
        }   
    }

    IEnumerator Death()
    {
        dyingAnim = true;
        GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("SpaceShip");
        health = maxHealth;
        oxygen = totalOxygen;
        GetComponent<Animator>().SetTrigger("FadeIn");
        dyingAnim = false;
    }

}
