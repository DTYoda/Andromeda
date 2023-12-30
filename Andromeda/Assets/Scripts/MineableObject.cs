using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : MonoBehaviour
{
    public string objName;
    public float totalHealth;
    public float currentHealth;
    public int hardness;
    public string itemDrop;

    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && manager != null)
        {
            manager.materials[itemDrop]++;
            Destroy(this.gameObject);
        }
    }
}
