using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altar_nv2 : MonoBehaviour
{

    public GameObject enemy1, enemy2, enemy3, enemy4, enemy5, enemy6;

    public GameObject cover1, cover2;

    public Vector3 cover1Pos, cover2Pos;

    bool playerInside;

    Vector3 initialPosCover1, initialPosCover2;

    GameObject[] enemies;

    int numEnemies;

    private void Awake()
    {
        enemies = new GameObject[] { enemy1, enemy2, enemy3, enemy4, enemy5, enemy6 };
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
        if (numEnemies <= 0 && playerInside)
        {
            Behaviour halo1 = (Behaviour) cover1.GetComponent("Halo");
            Behaviour halo2 = (Behaviour) cover2.GetComponent("Halo");
            halo1.enabled = false;
            halo2.enabled = false;
            cover1.transform.position = initialPosCover1;
            cover2.transform.position = initialPosCover2;
            //Iluminar altar
            //Y contar una sala completada
        }
    }

    public void enemyKilled()
    {
        numEnemies--;
    }

    public void playerEntered()
    {
        if (!playerInside)
        {
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
