using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isPaused = false;
    private CursorLockMode originalLockMode;
    private float originalTimescale;
    private bool originalCursorVisible;

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "Mainmenu" && Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            if (isPaused) Unpause(); else Pause();
        }
    }

    public void Pause()
    {
        originalLockMode = Cursor.lockState;
        originalTimescale = Time.timeScale;
        originalCursorVisible = Cursor.visible;

        isPaused = true;

        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        isPaused = false;

        pauseMenu.SetActive(false);
        Cursor.lockState = originalLockMode;
        Time.timeScale = originalTimescale;
        Cursor.visible = originalCursorVisible;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Mainmenu");
        Destroy(this.gameObject);

        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }
}
