using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuScript : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle musicToggle;
    public Toggle soundEffectToggle;
    public Toggle performanceToggle;
    public Toggle fullscreenToggle;
    public TMP_Dropdown graphicsDropDown;

    public Slider sensSlider;
    public TMP_Text sensInfo;

    public Slider fovSlider;
    public TMP_Text fovInfo;

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasOpenedGame") == 0)
        {
            PlayerPrefs.SetFloat("volume", 1);
            PlayerPrefs.SetInt("musicToggle", 1);
            PlayerPrefs.SetInt("soundEffectToggle", 1);
            PlayerPrefs.SetInt("HasOpenedGame", 1);
            PlayerPrefs.SetInt("fullscreen", 1);
            PlayerPrefs.SetInt("GraphicsLevel", 2);
            PlayerPrefs.SetInt("FOV", 60);
            PlayerPrefs.SetInt("sens", 50);
        }

        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        sensSlider.value = PlayerPrefs.GetInt("sens");
        fovSlider.value = PlayerPrefs.GetInt("FOV");


        musicToggle.isOn = PlayerPrefs.GetInt("musicToggle") == 1 ? true : false;
        soundEffectToggle.isOn = PlayerPrefs.GetInt("soundEffectToggle") == 1 ? true : false;
        performanceToggle.isOn = PlayerPrefs.GetInt("performanceMode") == 1 ? true : false;

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GraphicsLevel"));
        graphicsDropDown.value = QualitySettings.GetQualityLevel();

        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void ChangeVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void ChangeMusicToggle()
    {
        PlayerPrefs.SetInt("musicToggle", musicToggle.isOn ? 1 : 0);
    }

    public void ChangeSoundToggle()
    {
        PlayerPrefs.SetInt("soundEffectToggle", soundEffectToggle.isOn ? 1 : 0);
    }

    public void ChangeGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(graphicsDropDown.value);
        PlayerPrefs.SetInt("GraphicsLevel", graphicsDropDown.value);
    }

    public void ChangePerformanceMode()
    {
        PlayerPrefs.SetInt("performanceMode", performanceToggle.isOn ? 1 : 0);
    }

    public void ChangeFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }

    public void ChangeSens()
    {
        PlayerPrefs.SetInt("sens", (int) sensSlider.value);
        sensInfo.text = sensSlider.value.ToString();
    }

    public void ChangeFOV()
    {
        PlayerPrefs.SetInt("FOV", (int) fovSlider.value);
        fovInfo.text = fovSlider.value.ToString();
    }
}
