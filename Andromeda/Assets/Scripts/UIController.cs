using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text actionText;

    public static UIController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void setActionText(string s)
    {
        actionText.text = s;
    }
}
