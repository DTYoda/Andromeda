using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    private GameObject player;
    public Material skyShader;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(player.transform.position.y >= -10 && player.transform.position.y <= 10)
        {
            GetComponent<AudioSource>().pitch = (player.transform.position.y + 30) * 0.5f / 20f;
            skyShader.SetColor("_TintColor", new Color(0.6f + 0.04f * player.transform.position.y, 0.6f + 0.04f * player.transform.position.y, 0.6f + 0.04f * player.transform.position.y));
        }
        else
        {
            GetComponent<AudioSource>().pitch = Mathf.Sign(player.transform.position.y) == -1 ? 0.5f: 1;
            skyShader.SetColor("_TintColor", Mathf.Sign(player.transform.position.y) == -1 ? new Color(0.2f, 0.2f, 0.2f) : Color.white);
        }
    }
}
