using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInfoScript : MonoBehaviour
{
    public float totalHealth = 100;
    public float currentHealth = 100;
    public string animalName;
    public string animalDrop;
    public string animalType;

    public GameObject explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            AnimalSpawner.Instance.spawnedAnimals.Remove(this.transform);
            GameObject particles = Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
            particles.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            particles.GetComponent<AudioSource>().volume /= 2;
            particles.GetComponent<AudioSource>().Play();
            CameraShake.Instance.shake(0.2f, 1.5f);
            Destroy(this.gameObject);
        }
    }
}
