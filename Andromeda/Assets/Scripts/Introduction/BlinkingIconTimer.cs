using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingIconTimer : MonoBehaviour
{
    private float WASDtimer = 2;
    public GameObject wasdIcon;

    private float spaceTimer = 2;
    public GameObject spaceIcon;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            spaceTimer -= Time.deltaTime;
        }

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            WASDtimer -= Time.deltaTime;
        }

        if(WASDtimer <= 0)
        {
            wasdIcon.SetActive(false);
        }
        if(spaceTimer <= 0)
        {
            spaceIcon.SetActive(false);
        }
    }
}
