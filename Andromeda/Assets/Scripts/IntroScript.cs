using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.Find("FadeCanvas").GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(48);
        GameObject.Find("GameManager").GetComponent<GameManager>().currentPlanet = 2;
        GameObject.Find("GameManager").GetComponent<GameManager>().unlockedPlanets[1] = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().isInSpaceShip = false;
        GameObject.Find("GameManager").GetComponent<GameManager>().SaveData();
        SceneManager.LoadScene("Planet1");
    }
}
