using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileLauncher : MonoBehaviour
{
    private bool isShooting;

    public GameObject projectile;
    public float shootDelay;
    public float shootForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !isShooting)
        { 
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(projectile, this.transform.position, Camera.main.transform.rotation).GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * shootForce);
        yield return new WaitForSeconds(shootDelay);
        isShooting = false;
    }
}
