using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableObject : MonoBehaviour
{
    public float health;
    public int hardness;
    public string itemDrop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
