using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;
using TMPro;

public class player_combat : MonoBehaviour
{
    [Header("Pause Menu")]
    pause_menu pauseMenu;

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
    public GameObject muzzleFlash;
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
        pauseMenu = FindObjectOfType<pause_menu>();
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
        ammoText.GetComponent<TextMeshProUGUI>().text = bulletsLeft + "/" + magazineSize; 
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.GetComponent<TextMeshProUGUI>().text = bulletsLeft + "/" + magazineSize;

        if (pauseMenu.GetIsPaused()) { return; }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DamagePlayer();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            healPlayer(1);
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Tas muelto pibe");
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            pauseMenu.RespawnMenu();
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
            if ((Input.GetKeyDown(KeyCode.R) && !reloading && bulletsLeft < magazineSize && readyToShoot) || (Input.GetMouseButtonDown(0) && !reloading && bulletsLeft <= 0 && readyToShoot))
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
        muzzleFlash.SetActive(true);
        if (Physics.Raycast(camara.transform.position, camara.transform.forward, out rayHit, range, isEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy")) {
                hitmarker.SetActive(true);
                hitmarker.GetComponent<AudioSource>().Play();
                if (rayHit.collider.TryGetComponent<t800_soul>(out t800_soul soul800))
                {
                    soul800.RecibeDamage(damage);
                }
                else if (rayHit.collider.TryGetComponent<t200_soul>(out t200_soul soul200))
                {
                    soul200.RecibeDamage(damage);
                }
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
        muzzleFlash.SetActive(false);
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
        //if (other.tag == "Projectile")
        //{
        //    playerHealth--;
        //    ChangeHealthBars();
        //}
        if (other.tag == "Enemy")
        {
            DamagePlayer();
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
                color = new Color(0f, 1f, 0f);
                break;
            case 4:
                Debug.Log("Ahora 4");
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 0.15f);
                color = new Color(0.82f, 1f, 0f);
                break;
            case 3:
                Debug.Log("Ahora 3");
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 0.5f);
                color = new Color(1f, 0.78f, 0f);
                break;
            case 2:
                Debug.Log("Ahora 2");
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 0.75f);
                color = new Color(1f, 0.33f, 0f);
                break;
            default:
                lowHealth.GetComponent<RawImage>().color = new Color(lowHealth.GetComponent<RawImage>().color.r, lowHealth.GetComponent<RawImage>().color.g, lowHealth.GetComponent<RawImage>().color.b, 1f);
                color = new Color(1f, 0f, 0f);
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

    public void Respawn()
    {
        playerHealth = healthBars.Length;
        ChangeHealthBars();
        transform.position = respawnPoint.transform.position;
        altar_nv2 rp = respawnPoint.GetComponentInParent<altar_nv2>();
        if (rp != null)
        {
            rp.RespawnReset();
        }
    }

    public void RespawnChange(GameObject newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}
