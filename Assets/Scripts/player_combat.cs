using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public class player_combat : MonoBehaviour
{
    [Header("Health")]
    public int playerHealth;
    public GameObject[] healthBars;
    public GameObject lowHealth;
    public GameObject respawnPoint;
    Color color;

    public GameObject arma;
    public GameObject guadana;
    public Camera camara;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;
    Animator guadanaAnim;
    Animator pistolaAnim;

    [Header("Shooting")]
    public GameObject hitmarker;
    public GameObject ammoText;
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime;
    public int magazineSize;

    int bulletsLeft;

    //estados
    bool shooting, readyToShoot, reloading;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = healthBars.Length;
        readyToShoot = true;
        bulletsLeft = magazineSize;
        //Invoke(nameof(Prueba), 2f);//////////
        arma.SetActive(true);
        pistolaAnim = arma.GetComponent<Animator>();
        //guadana = WeaponHolder.gameObject.transform.GetChild(0).gameObject;
        guadana.SetActive(false);
        guadanaAnim = guadana.GetComponent<Animator>();
        ammoText.GetComponent<Text>().text = bulletsLeft + "/" + magazineSize; 
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.GetComponent<Text>().text = bulletsLeft + "/" + magazineSize;

        if (playerHealth <= 0)
        {
            Debug.Log("Tas muelto pibe");
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            playerHealth = healthBars.Length;
            ChangeHealthBars();
            transform.position = respawnPoint.transform.position;
            respawnPoint.GetComponentInParent<altar_nv2>().RespawnReset();
        }

        if(arma.activeSelf == true)
        {
            if (readyToShoot && Input.GetMouseButtonDown(0) && !reloading && bulletsLeft > 0)
            {
                Shoot();
            }
            else if (readyToShoot && Input.GetMouseButtonDown(0) && !reloading)
            {
                AudioSource sonido = arma.GetComponents<AudioSource>()[0];
                sonido.Play();
            }
            if (Input.GetKeyDown(KeyCode.R) && !reloading && bulletsLeft < magazineSize)
            {
                Reload();
            }
        }

        if (Input.GetMouseButtonDown(1) && !reloading && readyToShoot)
        {
            pistolaAnim.enabled = false;
            arma.SetActive(false);
            guadana.SetActive(true);
            guadanaAnim.Play("MeleeGuadana");
            Invoke(nameof(MeleeReset), 0.7f);
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        if (Physics.Raycast(camara.transform.position, camara.transform.forward, out rayHit, range, isEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy")) {
                hitmarker.SetActive(true);
                hitmarker.GetComponent<AudioSource>().Play();
                rayHit.collider.GetComponent<t800_soul>().RecibeDamage(damage);
            }
        }

        bulletsLeft -= 3;

        pistolaAnim.Play("Fuego1_Shot");
        AudioSource sonido = arma.GetComponents<AudioSource>()[1];
        sonido.Play();

        Invoke(nameof(ShootReset), timeBetweenShooting);
    }

    private void Reload()
    {
        reloading = true;
        pistolaAnim.Play("Fuego1_Reload");
        AudioSource sonido = arma.GetComponents<AudioSource>()[2];
        sonido.Play();
        bulletsLeft = magazineSize;
        Invoke(nameof(ReloadReset), 0.7f);
    }

    private void ShootReset()
    {
        readyToShoot = true;
        hitmarker.SetActive(false);
    }

    private void ReloadReset()
    {
        reloading = false;
    }

    private void MeleeReset()
    {
        arma.SetActive(true);
        pistolaAnim.enabled = true;
        guadana.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            playerHealth--;
            ChangeHealthBars();
        }
    }

    public void DamagePlayer()
    {
        GetComponents<AudioSource>()[5].Play();
        playerHealth = Mathf.Clamp(playerHealth - 1, 0, 5);
        ChangeHealthBars();
    }

    public void healPlayer(int healAmount)
    {
        playerHealth = Mathf.Clamp(playerHealth + healAmount, 0, 5);
        ChangeHealthBars();
    }

    private void ChangeHealthBars()
    {
        //Color color;
        switch (playerHealth)
        {
            case 5:
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 0f);
                color = new Color(0f, 255f, 0f);
                break;
            case 4:
                Debug.Log("Ahora 4");
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 63f);
                color = new Color(210f, 255f, 0f);
                break;
            case 3:
                Debug.Log("Ahora 3");
                //lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 127);
                color = new Color(255f, 200f, 0f);
                break;
            case 2:
                Debug.Log("Ahora 2");
                //lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 190);
                color = new Color(255f, 85f, 0f);
                break;
            default:
               // lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 255);
                color = new Color(255f, 0f, 0f);
                break;
        }
        for (int i = 0; i < playerHealth; i++)
        {
            healthBars[i].GetComponent<RawImage>().color = color;
            healthBars[i].SetActive(true);
        }
        for(int i = playerHealth; i < healthBars.Length; i++)
        {
            healthBars[i].SetActive(false);
        }
    }

    public void RespawnChange(GameObject newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}
