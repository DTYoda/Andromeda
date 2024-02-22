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
            Destroy(this.gameObject);
        }
    }
}
