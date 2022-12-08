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

    public GameObject arma;
    public GameObject guadana;
    public Camera camara;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;
    Animator guadanaAnim;
    Animator pistolaAnim;

    [Header("Shooting")]
    public GameObject ammoText;
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime;
    public int magazineSize;

    int bulletsLeft;

    //estados
    bool shooting, readyToShoot, reloading;



    // Start is called before the first frame update
    void Start()
    {
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
        }

        if (readyToShoot && Input.GetMouseButtonDown(0) && !reloading && bulletsLeft > 0)
        {
            Shoot();
        } else if (readyToShoot && Input.GetMouseButtonDown(0) && !reloading)
        {
            AudioSource sonido = arma.GetComponents<AudioSource>()[0];
            sonido.Play();
        }

        if (Input.GetMouseButtonDown(1))
        {
            arma.SetActive(false);
            guadana.SetActive(true);
            guadanaAnim.Play("MeleeGuadana");
            Invoke(nameof(MeleeReset), 0.7f);
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading && bulletsLeft < magazineSize)
         {
            Reload();
            pistolaAnim.Play("Fuego1_Reload");
         }
    }

    private void Shoot()
    {
        readyToShoot = false;

        if ( Physics.Raycast(camara.transform.position, camara.transform.forward, out rayHit, range, isEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy")) {
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
        AudioSource sonido = arma.GetComponents<AudioSource>()[2];
        sonido.Play();
        bulletsLeft = magazineSize;
    }

    private void ShootReset()
    {
        readyToShoot = true;
    }

    private void MeleeReset()
    {
        arma.SetActive(true);
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

    public void healPlayer(int healthChange)
    {
        playerHealth += healthChange;
    }

    private void ChangeHealthBars()
    {
        Color color;
        switch (playerHealth)
        {
            case 5:
                color = new Color(0, 255, 0);
                break;
            case 4:
                color = new Color(210, 255, 0);
                break;
            case 3:
                color = new Color(255, 200, 0);
                break;
            case 2:
                color = new Color(255, 85, 0);
                break;
            default:
                color = new Color(255, 0, 0);
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

    //public void Prueba()
    //{
    //    playerHealth--;
    //    ChangeHealthBars();
    //    Invoke(nameof(Prueba), 2f);
    //}
}
