using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_combat : MonoBehaviour
{

    public float playerHealth;

    public GameObject arma;
    public GameObject guadana;
    public Camera camara;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask isEnemy;
    Animator guadanaAnim;
    Animator pistolaAnim;

    //Disparos stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime;
    public int magazineSize;
    int bulletsLeft;

    //estados
    bool shooting, readyToShoot, reloading;


    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        bulletsLeft = magazineSize;

        arma.SetActive(true);
        pistolaAnim = arma.GetComponent<Animator>();
        //guadana = WeaponHolder.gameObject.transform.GetChild(0).gameObject;
        guadana.SetActive(false);
        guadanaAnim = guadana.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            Debug.Log("Tas muelto pibe");
        }

        if (readyToShoot && Input.GetMouseButtonDown(0) && !reloading && bulletsLeft > 0)
        {
            Debug.Log("hola");
            Shoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            arma.SetActive(false);
            guadana.SetActive(true);
            guadanaAnim.Play("MeleeGuadana");
            Invoke(nameof(MeleeReset), 0.7f);
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading && bulletsLeft < magazineSize) Reload();
    }

    private void Shoot()
    {
        readyToShoot = false;

        pistolaAnim.Play("Fuego1_Shot");

        Invoke(nameof(ShootReset), timeBetweenShooting);
    }

    private void Reload()
    {

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
            playerHealth -= 10;
        }
    }

    public void healPlayer(float healthChange)
    {
        playerHealth += healthChange;
    }
}
