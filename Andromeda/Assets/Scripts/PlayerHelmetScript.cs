using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelmetScript : MonoBehaviour
{
    public GameObject oxygenBar;
    public float totalOxygen;
    public float oxygen;

    public GameObject healthBar;
    public float totalHealth;
    public float health;

    public GameObject fuelBar;
    public float totalFuel;
    public float fuel;

    private GameManager manager;
    private PlayerArmController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerArmController>();
    }

    private void Update()
    {

        totalFuel = controller.maxArmFuel;
        fuel = controller.armFuel;

        //oxygenBar.transform.localScale = new Vector3(oxygen / totalOxygen, 1f, 1f);
        //healthBar.transform.localScale = new Vector3(health / totalHealth, 1f, 1f);
        fuelBar.transform.localScale = new Vector3(fuel / totalFuel, 1f, 1f);
    }
}
