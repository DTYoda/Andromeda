using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DangerTextScript : MonoBehaviour
{
    public TMP_Text dangerText;
    private PlayerController controller;
    private PlayerArmController armController;

    public List<string> dangers = new List<string>();
    private bool isWriting = false;

    private void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        armController = GameObject.Find("Player").GetComponent<PlayerArmController>();
    }

    private void Update()
    {
        //various error messages for the display to show the player

        //If the player is attempting to mine something too far away
        if (controller.currentObject != null && !armController.mineParticles.activeSelf && armController.isAttacking && !dangers.Contains("TOO FAR"))
        {
            dangers.Add("TOO FAR");
        }
        else if(!(controller.currentObject != null && !armController.mineParticles.activeSelf && armController.isAttacking) && dangers.Contains("TOO FAR"))
        {
            dangers.Remove("TOO FAR");
        }

        //if the player is on the dark side of the planet
        if(controller.gameObject.transform.position.x < 0 && !dangers.Contains("DANGER"))
        {
            dangers.Add("DANGER");
        }
        else if(!(controller.gameObject.transform.position.x < 0) &&dangers.Contains("DANGER"))
        {
            dangers.Remove("DANGER");
        }

        //If the player is low on guantlet fuel
        if(armController.armFuel <= 10 && !dangers.Contains("LOW FUEL"))
        {
            dangers.Add("LOW FUEL");
        }
        else if (!(armController.armFuel <= 10) && dangers.Contains("LOW FUEL"))
        {
            dangers.Remove("LOW FUEL");
        }

        //If the player is attmepting to mine something they cannot
        if(armController.mineObj != null && armController.mineObj.hardness > armController.miningForce && armController.isAttacking && !dangers.Contains("NOT STRONG ENOUGH"))
        {
            dangers.Add("NOT STRONG ENOUGH");
        }
        else if((armController.mineObj == null || (armController.mineObj != null && armController.mineObj.hardness <= armController.miningForce) || !armController.isAttacking) && dangers.Contains("NOT STRONG ENOUGH"))
        {
            dangers.Remove("NOT STRONG ENOUGH");
        }

       
        if (dangers.Count != 0 && !isWriting)
        {
            StartCoroutine(CycleDanger());
        }
        else if (dangers.Count == 0)
        {
            dangerText.text = "";
            StopAllCoroutines();
            isWriting = false;
        }
    }

    IEnumerator CycleDanger()
    {
        isWriting = true;
        for(int i = 0; i < dangers.Count; i++)
        {
            dangerText.text = "<sprite index=1>" + dangers[i] + "<sprite index=1>";
            yield return new WaitForSeconds(1);
        }
        isWriting = false;
    }
}
