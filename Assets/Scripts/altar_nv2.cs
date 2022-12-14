using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altar_nv2 : MonoBehaviour
{

    public GameObject musicObject;

    [Header("Puertas de entrada")]
    public GameObject cover1;
    public GameObject cover2;

    [Header("Puertas de final")]
    public GameObject endCover1;
    public GameObject endCover2;
    public GameObject exitIndicator;

    public Vector3 cover1Pos, cover2Pos;

    public GameObject MainCover;

    public GameObject ParticleSystem;

    [Header("Salas")]
    public GameObject[] salas;

    bool playerInside, altarComplete = false;

    Vector3 initialPosCover1, initialPosCover2;

    public GameObject[] enemies;

    int numEnemies;

    private void Awake()
    {
        numEnemies = enemies.Length;
        initialPosCover1 = cover1.transform.position;
        initialPosCover2 = cover2.transform.position;
        playerInside = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0 && playerInside && !altarComplete)
        {
            musicObject.GetComponents<AudioSource>()[0].enabled = true;
            musicObject.GetComponents<AudioSource>()[1].enabled = false;
            //Behaviour halo1 = (Behaviour) cover1.GetComponent("Halo");
            //Behaviour halo2 = (Behaviour) cover2.GetComponent("Halo");
            //halo1.enabled = false;
            //halo2.enabled = false;
            //cover1.transform.position = initialPosCover1;
            //cover2.transform.position = initialPosCover2;
            cover1.SetActive(false);
            cover2.SetActive(false);
            ParticleSystem.SetActive(true);
            altarComplete = true;
            MainCover.gameObject.GetComponent<sala_principal_nv2>().AltarCompleted();
            exitIndicator.SetActive(true);

            //Reactivación de las salas
            foreach (GameObject sala in salas)
            {
                sala.SetActive(true);
            }
        }
    }

    public void enemyKilled()
    {
        numEnemies--;
    }

    public void playerEntered()
    {
        if (!playerInside && !altarComplete)
        {

            //Desactivación de las salas
            foreach (GameObject sala in salas)
            {
                sala.SetActive(false);
            }
            this.gameObject.SetActive(true);
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
            }
            musicObject.GetComponents<AudioSource>()[0].enabled = false;
            musicObject.GetComponents<AudioSource>()[1].enabled = true;
            //cover1.transform.position = cover1Pos;
            //cover2.transform.position = cover2Pos;
            //Behaviour halo1 = (Behaviour)cover1.GetComponent("Halo");
            //Behaviour halo2 = (Behaviour)cover2.GetComponent("Halo");
            //halo1.enabled = true;
            //halo2.enabled = true;
            cover1.SetActive(true);
            cover2.SetActive(true);
            playerInside = true;
        }
    }

    public void RespawnReset()
    {
        if (!altarComplete)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
                enemy.GetComponent<t800_soul>().Respawn();
            }
            numEnemies = enemies.Length;
        }
    }

    public void CloseRoom()
    {
        if (altarComplete)
        {
            endCover1.SetActive(true);
            endCover2.SetActive(true);
        }
    }

}
