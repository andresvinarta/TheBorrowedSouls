using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altar_nv2 : MonoBehaviour
{

    public GameObject musicObject;

    public GameObject cover1, cover2;

    public Vector3 cover1Pos, cover2Pos;

    public GameObject MainCover;

    public GameObject ParticleSystem;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0 && playerInside && !altarComplete)
        {
            musicObject.GetComponents<AudioSource>()[0].enabled = true;
            musicObject.GetComponents<AudioSource>()[1].enabled = false;
            Behaviour halo1 = (Behaviour) cover1.GetComponent("Halo");
            Behaviour halo2 = (Behaviour) cover2.GetComponent("Halo");
            halo1.enabled = false;
            halo2.enabled = false;
            cover1.transform.position = initialPosCover1;
            cover2.transform.position = initialPosCover2;
            ParticleSystem.SetActive(true);
            altarComplete = true;
            MainCover.gameObject.GetComponent<sala_principal_nv2>().AltarCompleted();
        }
    }

    public void enemyKilled()
    {
        numEnemies--;
        Debug.Log("AAAAAAAAA");
    }

    public void playerEntered()
    {
        if (!playerInside)
        {
            musicObject.GetComponents<AudioSource>()[0].enabled = false;
            musicObject.GetComponents<AudioSource>()[1].enabled = true;
            cover1.transform.position = cover1Pos;
            cover2.transform.position = cover2Pos;
            Behaviour halo1 = (Behaviour)cover1.GetComponent("Halo");
            Behaviour halo2 = (Behaviour)cover2.GetComponent("Halo");
            halo1.enabled = true;
            halo2.enabled = true;
            playerInside = true;
        }
    }


}
