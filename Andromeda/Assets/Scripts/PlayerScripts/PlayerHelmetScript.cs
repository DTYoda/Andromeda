using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelmetScript : MonoBehaviour
{
    public GameObject oxygenBar;
    [System.NonSerialized] public float totalOxygen = 300f;
    [System.NonSerialized] public float oxygen = 300f;

    public GameObject healthBar;
    [System.NonSerialized] public float totalHealth = 100f;
    [System.NonSerialized] public float health = 100f;

    public GameObject fuelBar;
    [System.NonSerialized] public float totalFuel = 40f;
    [System.NonSerialized] public float fuel = 40f;

    private GameManager manager;

    private void Start()
    {
        
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            
            health = manager.health;
            totalHealth = manager.maxHealth;
            totalFuel = manager.maxArmFuel;
            fuel = manager.armFuel;
        }
    }

    private void Update()
    {

        if (GameObject.Find("GameManager") != null)
        {
            totalOxygen = manager.totalOxygen;
            oxygen = manager.oxygen;
            health = manager.health;
            totalHealth = manager.maxHealth;
            totalFuel = manager.maxArmFuel;
            fuel =  manager.armFuel;
        }
        if(GameObject.Find("GameManager") != null)
        {
            if (oxygen > 0)
            {
                manager.oxygen -= Time.deltaTime;
            }
            else
            {
                manager.health -= 5 * Time.deltaTime;
            }
        }
        
        

        

        oxygenBar.transform.localScale = new Vector3(oxygen / totalOxygen, 1f, 1f);
        healthBar.transform.localScale = new Vector3(health / totalHealth, 1f, 1f);
        fuelBar.transform.localScale = new Vector3(fuel / totalFuel, 1f, 1f);
    }
}
