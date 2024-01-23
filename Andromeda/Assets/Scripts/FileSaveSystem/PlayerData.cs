using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public int currentPlanet;
    public bool[] unlockedPlanets;
    public float[] location = new float[3];
    public float armFuel;
    public float maxArmFuel;

    public string destroyedObjets;

    public PlayerData(GameManager manager)
    {
        health = manager.health;
        currentPlanet = manager.currentPlanet;
        unlockedPlanets = manager.unlockedPlanets;
        destroyedObjets = manager.destroyedObjects;
        armFuel = manager.armFuel;
        maxArmFuel = manager.maxArmFuel;

        location[0] = manager.playerLocation.x;
        location[1] = manager.playerLocation.y;
        location[2] = manager.playerLocation.z;
    }
}
