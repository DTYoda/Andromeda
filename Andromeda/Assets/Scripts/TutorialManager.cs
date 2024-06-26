using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private string introText = "Kendrick. This is a prerecorded message set to be played in the case of a crash. If you're hearing this, you have crashlanded in an unknown location and need to repair communication with us";
    public string[] direction;
    public string displayedText;
    public int currentStep = 0;
    public int isTyping = 0;
    private bool isInTutorial;

    private GameManager manager;
    public TMP_Text tutorialText;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        isInTutorial = (manager.saveFile != "" && !manager.completedTutorial && manager.unlockedPlanets[1] && SceneManager.GetActiveScene().name != "Mainmenu");

        tutorialText.text = displayedText;

        if (isInTutorial)
        {
            switch (currentStep)
            {
                case (0):
                    Step0();
                    break;
                case (1):
                    Step1();
                    break;
                case (2):
                    Step2();
                    break;
                case (3):
                    Step3();
                    break;
                case (4):
                    Step4();
                    break;
                case (5):
                    Step5();
                    break;
                case (6):
                    Step6();
                    break;
                case (7):
                    Step7();
                    break;
            }
        }
        
    }

    public void Step0()
    {
        if(GameObject.Find("Spaceship") != null)
        {
            GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship = false;
        }
        direction = introText.Split(".");
        if(isTyping == 0)
            StartCoroutine(TypeText());
        if(isTyping == 1)
        {
            currentStep = 1;
            isTyping = 0;
        }

    }

    private float walkTime = 0;
    public void Step1()
    {
        direction = new string[]{"First, you'll need to explore your environment to get a sense of your surroundings.", "Walk around for a little bit."};
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1)
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                walkTime += Time.deltaTime;
            }
            if(walkTime >= 20)
            {
                currentStep = 2;
                isTyping = 0;
            }
        }
    }

    public void Step2()
    {
        direction = new string[]{"You need to get materials to repair communication, but you'll need to upgrade your gauntlet first.", "Your gauntlet allows you to fight off enemies and mine objects such as trees and rocks.", "Right now, your Gauntlet can't do much, so you'll need to upgrade it", "Collect 20 red mushrooms off the ground to upgrade it."};
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1 && manager.getItemAmount("mushroom") >= 20)
        {
            currentStep = 3;
            isTyping = 0;
        }
    }

    public void Step3()
    {
        direction = new string[] {"Now that you have the materials, you need to make your way into your spaceship and use the upgrade center to upgrade your Gauntlet's strength."};
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1)
        {
            if (GameObject.Find("Spaceship") != null)
            {
                GameObject.Find("Spaceship").GetComponent<ExitSpaceShipScript>().canEnterSpaceship = true;
            }
            if (manager.isInSpaceShip)
            {
                currentStep = 4;
                isTyping = 0;
            }
        }
    }

    public void Step4()
    {
        direction = new string[] { "The upgrade center can be used to upgrade your spaceship and spacesuit.", "Make sure to consistantly upgrade your gear, or you may not be able to survive for long", "Go to the upgrade center, select your gauntlet, and Upgrade your Gauntlet Strength." };
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1 && manager.getUpgradeLevel("Mining Strength") >= 1)
        {
            currentStep = 5;
            isTyping = 0;
        }
    }

    public void Step5()
    {
        direction = new string[] {"With your upgraded Gauntlet, you can now destroy trees and rocks, the exact things you'll need to fix your Quantum Receptor.", "Mine rocks and trees, they will drop rock and wood that you must pick up. Collect 10 rocks and 10 pieces of wood."};
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1 && manager.getItemAmount("wood") >= 10 && manager.getItemAmount("rock") >= 10)
        {
            currentStep = 6;
            isTyping = 0;
        }
    }

    public void Step6()
    {
        direction = new string[] { "Return to your spaceship and repair your Quantum Receptor at the upgrade center by selecting \"ship\" and then \"communication\"" };
        if (isTyping == 0)
            StartCoroutine(TypeText());
        if (isTyping == 1 && manager.getUpgradeLevel("Quantum Receptor") >= 1)
        {
            isTyping = 0;
            currentStep = 7;
        }
    }

    public void Step7()
    {
        direction = new string[] { "Kendrick! I'm glad you were able to repair the quantum receptor!", "Looks like the prerecorded message helped you out a bit.", "You can use this quantum receptor to contact us, and we can give you missions to help your escape.", "The only objective now is to escape. Find a way to repair your spaceship and get out", "Try finding and opening your Quantum Receptor and start your first mission" };
        if (isTyping == 0)
        {
            StartCoroutine(TypeText());
            StartCoroutine(Dissapear());
        }
            
        if (isTyping == 1 && manager.getUpgradeLevel("Quantum Receptor") >= 1)
        {
            isTyping = 0;
            manager.completedTutorial = true;
            displayedText = "";
        }
    }

    IEnumerator TypeText()
    {
        isTyping =  -1;
        displayedText = "";
        for(int j = 0; j < direction.Length; j++)
        {
            for (int i = 0; i < direction[j].Length; i++)
            {
                displayedText += direction[j][i];
                yield return new WaitForSecondsRealtime(0.025f);
            }
            yield return new WaitForSecondsRealtime(4);
            if(j != direction.Length - 1)
                displayedText = "";
        }
        isTyping = 1;
    }

    IEnumerator Dissapear()
    {
        yield return new WaitForSecondsRealtime(5);
        manager.activeQuest = "";

    }
}
