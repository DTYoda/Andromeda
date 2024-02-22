using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingScript : MonoBehaviour
{

    private GameManager manager;
    private bool isGrowing;

    public List<GameObject> destroyedObjects= new List<GameObject>();

    private void Start()
    {
        if(GameObject.Find("GameManager") != null)
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        if(!isGrowing && GameObject.Find("GameManager") != null)
        {
            StartCoroutine(RegrowObjects());
        }
    }

    IEnumerator RegrowObjects()
    {
        isGrowing = true;
        for (int i = 0; i < destroyedObjects.Count; i++)
        {
            if (Random.Range(0, 30) == 0)
            {
                destroyedObjects[i].SetActive(true);
                manager.destroyed.Remove(destroyedObjects[i].name);
                destroyedObjects.Remove(destroyedObjects[i]);
            }
        }
        yield return new WaitForSeconds(10);
        isGrowing = false;
    }
}
