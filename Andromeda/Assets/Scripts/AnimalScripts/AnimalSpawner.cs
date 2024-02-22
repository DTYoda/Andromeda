using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] passiveAnimals;
    public GameObject[] aggressiveAnimals;
    private bool isSpawningPassive;
    private bool isSpawningAggressive;
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
        if(!isSpawningPassive)
        {
            StartCoroutine(SpawnPassive());
        }
        if(!isSpawningAggressive)
        {
            StartCoroutine(SpawnHostile());
        }
    }

    IEnumerator SpawnPassive()
    {
        isSpawningPassive = true;
        if(Random.Range(0, 2) == 0)
        {
            Vector3 spawnLocation = Random.onUnitSphere * 80;

            if (spawnLocation.x >= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(spawnLocation, Vector3.zero - spawnLocation, out hit, 80, spawnmask) && maxAnimalCount > spawnedAnimals.Count)
                {
                    spawnedAnimals.Add(Instantiate(passiveAnimals[Random.Range(0, passiveAnimals.Length)], hit.point, this.transform.rotation, this.transform).transform);
                }
            }
        }
        yield return new WaitForSeconds(passiveSpawnDelay);
        isSpawningPassive = false;
    }
    IEnumerator SpawnHostile()
    {
        isSpawningAggressive = true;
        if (Random.Range(0, 3) == 0)
        {
            Vector3 spawnLocation = Random.onUnitSphere * 80;

            if (spawnLocation.x < 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(spawnLocation, Vector3.zero - spawnLocation, out hit, 80, spawnmask) && maxAnimalCount > spawnedEnemies.Count)
                {
                    spawnedEnemies.Add(Instantiate(aggressiveAnimals[Random.Range(0, aggressiveAnimals.Length)], hit.point, this.transform.rotation, this.transform).transform);
                }
            }

        }
        yield return new WaitForSeconds(aggressiveSpawnDelay / (player.transform.position.x < 0 ? 4 : 1) );
        isSpawningAggressive = false;
    }
}
