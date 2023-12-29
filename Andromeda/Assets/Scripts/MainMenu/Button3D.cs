using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    public AudioClip click;
    public AudioClip hover;

    public bool isBool;
    public bool setBoolTo;

    public bool isInt;
    public int setIntTo;

    private Animator anim;
    public string prop;

    public GameObject settingsMenu;

    private void Start()
    {
        anim = Camera.main.gameObject.GetComponent<Animator>();
    }
    private void OnMouseUpAsButton()
    {
        if(isBool)
            anim.SetBool(prop, setBoolTo);
        if (isInt)
            anim.SetInteger(prop, setIntTo);
        GetComponent<AudioSource>().volume = 0.7f;
        GetComponent<AudioSource>().clip = click;
        GetComponent<AudioSource>().Play();

        if(prop == "Settings")
        {
            StartCoroutine(Settings());
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale *= 1.4f;
        GetComponent<AudioSource>().volume = 0.4f;
        GetComponent<AudioSource>().clip = hover;
        GetComponent<AudioSource>().Play();
    }
    private void OnMouseExit()
    {
        transform.localScale /= 1.4f;
    }

    IEnumerator Settings()
    {
        if(setBoolTo == true)
        {
            yield return new WaitForSeconds(2);
            settingsMenu.SetActive(true);
        }
        else
        {
            settingsMenu.SetActive(false);
        }
        
    }
}
