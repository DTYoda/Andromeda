using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //saving data
    public float health;
    public float[] playerLocation = new float[3];
    public int currentPlanet;
    public bool isInSpaceShip;
    public bool[] unlockedPlanets;
    public List<string> destroyed;
    public bool completedTutorial;

    //ArmUpgrades
    public float armFuel;
    public float maxArmFuel;
    public float armDamage;
    public float armStrength;

    //HelmetUpgrades
    public float oxygen;
    public float totalOxygen;

    //Boot Upgrades
    public float jumpHeight;
    public float walkSpeed;
    public float multiJumpAmount;
    public bool canMultiJump;

    //Armor Upgrades
    public float maxHealth;
    public float healthRegen;
    public float defense;

    //material and upgrade data
    public int[] materialAmounts;
    public int[] upgradeLevels;

    //quest data
    public List<string> completedQuests;
    public List<string> QRQuests;
    public string activeQuest;
    public int activeQuestStep;
    public float astroXP;
    public int astroLevel;


    public PlayerData(GameManager manager)
    {
        //saving data
        health = manager.health;
        currentPlanet = manager.currentPlanet;
        unlockedPlanets = manager.unlockedPlanets;
        isInSpaceShip = manager.isInSpaceShip;

        playerLocation[0] = manager.playerLocation.x;
        playerLocation[1] = manager.playerLocation.y;
        playerLocation[2] = manager.playerLocation.z;

        destroyed = manager.destroyed;
        completedTutorial = manager.completedTutorial;
        

    //armupgrades
        armFuel = manager.armFuel;
        maxArmFuel = manager.maxArmFuel;
        armDamage = manager.armDamage;
        armStrength = manager.armStrength;

        //helmet upgrades
        oxygen = manager.oxygen;
        totalOxygen = manager.totalOxygen;

        //boot upgrades
        jumpHeight = manager.jumpHeight;
        walkSpeed = manager.walkSpeed;
        multiJumpAmount = manager.multiJumpAmount;
        canMultiJump = manager.canMultiJump;

        //armor upgrades
        maxHealth = manager.maxHealth;
        healthRegen = manager.healthRegen;
        defense = manager.defense;
        
        //material and upgrade data
        materialAmounts = manager.materialAmounts;
        upgradeLevels = manager.upgradeLevels;

        //quest data
        completedQuests = manager.completedQuests;
        QRQuests = manager.QRQuests;
        activeQuest = manager.activeQuest;
        activeQuestStep = manager.activeQuestStep;
        astroXP = manager.astroXP;
        astroLevel = manager.astroLevel;
    }
}
