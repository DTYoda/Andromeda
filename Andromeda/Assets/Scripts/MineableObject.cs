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

    public GameObject explosionParticles;

    private GameManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.Find("GameManager") != null)
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
            Vector3 orginPos = GameObject.Find("Player_Camera").transform.localPosition;
            CameraShake.Instance.shake(0.2f, 1);
            if(GameObject.Find("GameManager") != null)
                manager.materials[itemDrop]++;
            Destroy(this.gameObject);
        }
    }
}
