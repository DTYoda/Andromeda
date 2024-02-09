using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeScript : MonoBehaviour
{
    public GameObject textObject;
    public GameObject upgradeUI;
    public LayerMask mask;

    private GameManager manager;

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, mask))
        {
            textObject.SetActive(hit.transform.gameObject == this.gameObject);
            if(hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                upgradeUI.SetActive(true);
            }
        }
        else
        {
            textObject.SetActive(false);
        }
            
    }

    public void exitUpgrades()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        upgradeUI.SetActive(false);
    }

    public bool Upgrade(string[] materials, string[] amounts, string upgrade, float upgradeAmount)
    {
        int[] newAmounts = new int[amounts.Length];
        for(int j = 0; j < amounts.Length; j++)
        {
            newAmounts[j] = int.Parse(amounts[j]);
        }

        if(GameObject.Find("GameManager") != null)
        {
            if(manager.addItems(materials, newAmounts))
            {
                switch(upgrade)
                {
                    case ("Jump Height"):
                        manager.jumpHeight += upgradeAmount;
                        break;
                    case ("Walking Speed"):
                        manager.walkSpeed += upgradeAmount;
                        break;
                    case ("MultiJump"):
                        manager.multiJumpAmount += upgradeAmount;
                        manager.canMultiJump = true;
                        break;
                    case ("Max Fuel"):
                        manager.maxArmFuel += upgradeAmount;
                        break;
                    case ("Mining Strength"):
                        manager.armStrength += upgradeAmount;
                        break;
                    case ("Damage"):
                        manager.armDamage += upgradeAmount;
                        break;
                    case ("Max Health"):
                        manager.maxHealth += upgradeAmount;
                        break;
                    case ("Health Regen"):
                        manager.healthRegen += upgradeAmount;
                        break;
                    case ("Defense"):
                        manager.defense += upgradeAmount;
                        break;

                }
                return true;
            }
            return false;
        }
        return false;
    }
}
