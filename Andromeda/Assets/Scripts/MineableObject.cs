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

    public GameObject drop;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
            CameraShake.Instance.shake(0.2f, 1);

            for(int i = 0; i < Random.Range(1, 4); i++)
            {
                Instantiate(drop, transform.position + 3 * transform.up, this.transform.rotation).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(300, 700)));
            }    

            Destroy(this.gameObject);
        }
    }
}
