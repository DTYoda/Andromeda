using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [System.NonSerialized] public float damage;
    public GameManager manager;

    private void Start()
    {
        StartCoroutine(Kill());
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 30;


        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 1))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                MineableObject obj = hit.transform.gameObject.GetComponent<MineableObject>();
                if(obj.hardness <= manager.armStrength)
                    obj.currentHealth -= damage;
            }
            if (hit.transform.gameObject.layer == 9)
            {
                AnimalInfoScript obj = hit.transform.gameObject.GetComponent<AnimalInfoScript>();
                obj.currentHealth -= damage;
            }

            if (hit.transform.gameObject.layer != 3)
                Destroy(this.gameObject);
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
