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

    public string[] destroyedObjets;

    public PlayerData(GameManager manager)
    {
        health = manager.health;
        currentPlanet = manager.currentPlanet;
        unlockedPlanets = manager.unlockedPlanets;
        destroyedObjets = manager.destroyedObjects;

        location[0] = manager.playerLocation.x;
        location[1] = manager.playerLocation.y;
        location[2] = manager.playerLocation.z;
    }
}
