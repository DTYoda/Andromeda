using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    private bool hasStarted = false;
    public Animator anim;
    public GameObject startText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !hasStarted)
        {
            hasStarted = true;
            anim.SetTrigger("Start");
            startText.SetActive(false);
        }
    }

}
