using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Spaceship : MonoBehaviour
{
    private Rigidbody rb;

    //Helping variables
    private float pitch;
    private float yaw;
    private float roll;
    private float isLining = 3;
    private bool escaped;
    private string[] quests = { "Escape Astroid Belt", "Align with Andromeda Galaxy", "PRESS SPACE" };
    private int currentQuest = 0;

    //set these variables
    public float responsiveness;
    public float speed;

    //initiated variables
    private ParticleSystem explosion;
    private ParticleSystem thrust;
    public GameObject center;
    public GameObject centerArrow;
    public TMP_Text countdownText;
    public TMP_Text questText;

    // Start is called before the first frame update
    void Start()
    {
        //set variables
        rb = GetComponent<Rigidbody>();
        explosion = transform.Find("Explosion").GetComponent<ParticleSystem>();
        thrust = transform.Find("thrust").GetComponent<ParticleSystem>();
        if(GameObject.Find("GameManager") !=null)
            StartCoroutine(Begin());

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            speed += 2f * speed * Time.deltaTime;
            thrust.Play();
        }

        thrust.startSpeed = speed;

        if(speed >= 100f)
        {
            speed -= 1.1f * speed * Time.deltaTime;
        }

        pitch = Input.GetAxis("Vertical");
        roll = -Input.GetAxis("Horizontal");

        questText.text = quests[currentQuest];

        if (transform.eulerAngles.x >= 2 && transform.eulerAngles.x <= 8 && transform.eulerAngles.y >= 20 && transform.eulerAngles.y <= 26 && escaped)
        {
            isLining -= Time.deltaTime;
            if(isLining > 0)
            {
                countdownText.text = isLining.ToString("F1");
            }
            else
            {
                countdownText.text = "READY";
                currentQuest = 2;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(End());
                }
            }
        }
        else
        {
            isLining = 3;
            countdownText.text = "";
            if(escaped == true)
            {
                currentQuest = 1;
            }
        }

        center.transform.eulerAngles = new Vector3(5, 23, 0);


        if (center.activeSelf && !CheckVisibility())
        {
            centerArrow.SetActive(true);
            centerArrow.transform.LookAt(center.transform.GetChild(0).transform.position);
        }
        else
        {
            centerArrow.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
        rb.AddTorque(transform.right * pitch * responsiveness);
        rb.AddTorque(-rb.angularVelocity);
        rb.AddTorque(transform.up * yaw * responsiveness);
        rb.AddTorque(transform.forward * roll * responsiveness);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("astroid"))
        {
            explosion.Play();
            explosion.gameObject.GetComponent<AudioSource>().Play();
            CameraShake.Instance.shake(1, 1);
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            this.GetComponent<MeshRenderer>().enabled = false;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        escaped = true;
        center.SetActive(true);
        currentQuest = 1;
    }

    public bool CheckVisibility()
    {
        //Check Visibility

        Vector3 screenPos = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(center.transform.GetChild(0).transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

        return (onScreen && center.transform.GetChild(0).GetComponent<Renderer>().isVisible);
    }

    IEnumerator Begin()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(10);
        Time.timeScale = 1;
        GameObject.Find("GameManager").GetComponent<Animator>().SetTrigger("FadeIn");
    }

    IEnumerator End()
    {
        thrust.Stop();
        thrust.loop = true;
        CameraShake.Instance.shake(0.25f, 7);
        GetComponent<AudioSource>().Play();
        thrust.Play();
        yield return new WaitForSeconds(3);
        speed *= 10;
        GameObject.Find("GameManager").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(4);
        GameObject.Find("GameManager").GetComponent<GameManager>().currentPlanet = 2;
        GameObject.Find("GameManager").GetComponent<GameManager>().unlockedPlanets[1] = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().SaveData();
        SceneManager.LoadScene(2);
    }
}
