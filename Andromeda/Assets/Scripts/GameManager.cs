using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float health = 100;

    public Dictionary<string, int> materials = new Dictionary<string, int>();

    private void Start()
    {
        materials.Add("wood", 0);
    }

    private void Update()
    {
        
    }
}
