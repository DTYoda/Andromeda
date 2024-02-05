using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelmetScript : MonoBehaviour
{
    public GameObject oxygenBar;
    [System.NonSerialized] public float totalOxygen = 300f;
    [System.NonSerialized] public float oxygen = 300f;

    public GameObject healthBar;
    public float totalHealth;
    public float health;

    public GameObject fuelBar;
    [System.NonSerialized] public float totalFuel;
    [System.NonSerialized] public float fuel;

    private GameManager manager;
    private PlayerArmController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerArmController>();
        
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            totalOxygen = manager.totalOxygen;
            oxygen = manager.oxygen;
        }
    }

    private void Update()
    {

        if (GameObject.Find("GameManager") != null)
        {
            manager.totalOxygen = totalOxygen;
            manager.oxygen = oxygen;
        }

        oxygen -= Time.deltaTime;

        totalFuel = controller.maxArmFuel;
        fuel = controller.armFuel;

        oxygenBar.transform.localScale = new Vector3(oxygen / totalOxygen, 1f, 1f);
        //healthBar.transform.localScale = new Vector3(health / totalHealth, 1f, 1f);
        fuelBar.transform.localScale = new Vector3(fuel / totalFuel, 1f, 1f);
    }
}
