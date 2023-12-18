using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button3D : MonoBehaviour
{
    private Animator anim;
    public string prop;

    private void Start()
    {
        anim = Camera.main.gameObject.GetComponent<Animator>();
    }
    private void OnMouseUpAsButton()
    {
        anim.SetBool(prop, !anim.GetBool(prop));
    }
}
