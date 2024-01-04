using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    private void Update()
    {
        if(Camera.main.gameObject.GetComponent<Animator>().GetBool("Settings"))
        {

        }
    }
}
