using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] passiveAnimals;
    public GameObject[] aggressiveAnimals;
    public float passiveSpawnDelay;
    public float aggressiveSpawnDelay;
    public List<Transform> spawnedAnimals = new List<Transform>();
    public List<Transform> spawnedEnemies = new List<Transform>();
    public int maxAnimalCount;

    public LayerMask spawnmask;

    private GameObject player;

    public static AnimalSpawner Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(maxAnimalCount > spawnedAnimals.Count)
        {
            SpawnPassive();
        }
        if((player.transform.position.y < 0 ? maxAnimalCount : maxAnimalCount / 3) > spawnedEnemies.Count)
        {
            SpawnHostile();
        }
    }

    public void SpawnPassive()
    {
        Vector3 spawnLocation = Random.onUnitSphere * 80;

        if (spawnLocation.y >= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(spawnLocation, Vector3.zero - spawnLocation, out hit, 80, spawnmask))
            {
                spawnedAnimals.Add(Instantiate(passiveAnimals[Random.Range(0, passiveAnimals.Length)], hit.point, this.transform.rotation, this.transform).transform);
            }
        }
    }
    public void SpawnHostile()
    {

        Vector3 spawnLocation = Random.onUnitSphere * 80;

        if (spawnLocation.y < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(spawnLocation, Vector3.zero - spawnLocation, out hit, 80, spawnmask))
            {
                spawnedEnemies.Add(Instantiate(aggressiveAnimals[Random.Range(0, aggressiveAnimals.Length)], hit.point, this.transform.rotation, this.transform).transform);
            }

        }
    }
}
