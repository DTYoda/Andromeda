using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorScript : MonoBehaviour
{

    public GameObject textObject;
    private bool isInside;

    [SerializeField] private ConfirmationWindow confirmWindow;

    private void Update()
    {
        if(isInside)
        {
            if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("GameManager") != null)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                confirmWindow.gameObject.SetActive(true);
                confirmWindow.messageText.text = "Exit spaceship?";
                confirmWindow.yesButton.onClick.RemoveAllListeners();
                confirmWindow.noButton.onClick.RemoveAllListeners();
                confirmWindow.yesButton.onClick.AddListener(yesExit);
                confirmWindow.noButton.onClick.AddListener(noExit);
                GameObject.Find("PopSound").GetComponent<AudioSource>().Play();
            }
            textObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
        }
    }

    private void yesExit()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.isInSpaceShip = false;
        SceneManager.LoadScene(manager.currentPlanet);
        confirmWindow.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void noExit()
    {
        Time.timeScale = 1;
        confirmWindow.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            textObject.SetActive(true);
            isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        textObject.SetActive(false);
        isInside = false;
    }
}
