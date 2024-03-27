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
    public bool reverseSpread = false;
    public float spreadStrength = 1;

    public GameObject explosionParticles;

    public GameObject drop;
    private GameManager manager;
    private GrowingScript growManager;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = totalHealth;
    }

    private void Start()
    {

        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            growManager = GameObject.Find("GrowManager").GetComponent<GrowingScript>();

            if (manager.destroyed.Contains(this.name))
            {
                growManager.destroyedObjects.Add(this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            manager = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0){
            GameObject particles = Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
            particles.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            particles.GetComponent<AudioSource>().Play();
            CameraShake.Instance.shake(0.5f, 1.5f);
            for (int i = 0; i < Random.Range(2, 5); i++)
            {
                Rigidbody dropRB = Instantiate(drop, this.transform.position + Random.Range(1.0f, 3.0f) * (this.transform.position - GameObject.Find("planet"). transform.position).normalized, this.transform.rotation).GetComponent<Rigidbody>();
                Vector3 forceRight = (Random.Range(-100, 100) * transform.right);
                Vector3 forceForward = (Random.Range(100, 300) * (this.transform.position - GameObject.Find("planet").transform.position).normalized);
                Vector3 forceUp = (Random.Range(-100, 100) * transform.up);
                dropRB.AddForce(forceRight + forceUp + forceForward);
                dropRB.AddTorque(forceRight * 3 + forceUp * 3 + forceForward);
            }
            if (manager != null)
            {
                manager.destroyed.Add(this.name);
                growManager.destroyedObjects.Add(this.gameObject);
            }

            currentHealth = totalHealth;
            gameObject.SetActive(false);
        }
            
    }
}
