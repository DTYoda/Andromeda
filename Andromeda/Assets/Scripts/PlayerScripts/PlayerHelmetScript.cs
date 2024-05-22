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

    //Inventory variables
    public GameObject inventory;
    private bool inventoryOpen = false;

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

        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            inventory.GetComponent<Animator>().SetBool("isOpen", inventoryOpen);
            healthBar.transform.parent.gameObject.GetComponent<Animator>().SetBool("isOpen", inventoryOpen); 
        }
        
        

        oxygenBar.transform.localScale = new Vector3((int) oxygen / totalOxygen, 1f, 1f);
        healthBar.transform.localScale = new Vector3((int) health / totalHealth, 1f, 1f);
        fuelBar.transform.localScale = new Vector3(fuel / totalFuel, 1f, 1f);
    }
}
