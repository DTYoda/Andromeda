using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitSpaceShipScript : MonoBehaviour
{
    public GameObject textObject;
    private GameObject currentTextObject;
    public bool canEnterSpaceship = true;

    public LayerMask mask;

    [SerializeField] private ConfirmationWindow confirmWindow;

    private void Update()
    {
        

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5, mask) && canEnterSpaceship)
        {
            if(currentTextObject == null)
            {
                currentTextObject = Instantiate(textObject);
                currentTextObject.transform.localScale *= 3;
            }
            currentTextObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
            currentTextObject.SetActive(hit.transform.gameObject == this.gameObject);
            currentTextObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
            currentTextObject.transform.position = this.transform.position + 2f * (this.transform.position - GameObject.Find("planet").transform.position).normalized;
            currentTextObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "spaceship";
            currentTextObject.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "press \"E\" to enter";
            if (hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E) && GameObject.Find("GameManager") != null && canEnterSpaceship)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                confirmWindow.gameObject.SetActive(true);
                confirmWindow.messageText.text = "Enter spaceship?";
                confirmWindow.yesButton.onClick.RemoveAllListeners();
                confirmWindow.noButton.onClick.RemoveAllListeners();
                confirmWindow.yesButton.onClick.AddListener(yesEnter);
                confirmWindow.noButton.onClick.AddListener(noEnter);
            }
            else if(hit.transform.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("SpaceShip");
                Destroy(currentTextObject);
            }
        }
        else if(currentTextObject != null)
        {
            Destroy(currentTextObject);
            currentTextObject = null;
        }

    }

    private void yesEnter()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.isInSpaceShip = true;
        SceneManager.LoadScene("SpaceShip");
        Destroy(currentTextObject);
        Time.timeScale = 1;
    }

    private void noEnter()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        confirmWindow.gameObject.SetActive(false);
    }
}
