using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class t800_soul : MonoBehaviour
{

    public NavMeshAgent t800;
    Animator t800Anim;

    public Transform player;

    public GameObject altar;

    public LayerMask isGround, isPlayer;


    //Idle-Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange, stunned;

    //Health and damage values
    public float health;
    public float maxHealth;
    public float reviveTime;
    float stunMoment;


    private void Awake()
    {
        player = GameObject.Find("Capsule").transform;
        t800 = GetComponent<NavMeshAgent>();
        t800Anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        stunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInSightRange && !playerInAttackRange && health>0) { Patroling(); t800Anim.SetBool("isWalkin", true); }
        else if (!playerInAttackRange && health > 0) { ChasePlayer(); t800Anim.SetBool("isWalkin", true); }
        else if (health > 0) { AttackPlayer(); t800Anim.SetBool("isWalkin", true); }
        else if (!stunned){ Stun(); t800Anim.SetBool("isWalkin", false); }
    }

    private void Patroling()
    {
        if (!walkPointSet) FindWalkPoint();
        else
        {
            t800.isStopped = false;
            t800.SetDestination(walkPoint); 
            Vector3 walkLook = new Vector3(walkPoint.x, transform.position.y, walkPoint.z);
            transform.LookAt(walkLook);
        }

        Vector3 distanceToWalk = transform.position - walkPoint;

        if (distanceToWalk.magnitude < 1f) walkPointSet = false;
    }

    private void FindWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, isGround)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        t800.isStopped = false;
        t800.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
    private void AttackPlayer()
    {
        t800.isStopped = false;
        t800.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));

        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPos);

        if (!alreadyAttacked)
        {
            //Coigo para el ataque (sera similar al de disparar)

            alreadyAttacked = true;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
        }
    }

    public void RecibeDamage(int damage)
    {
        health -= damage;
    }
    private void AttackReset()
    {
        alreadyAttacked = false;
    }

    public void Stun()
    {
        Debug.Log("T800 stunned");
        stunned = true;
        t800.isStopped = true;
        stunMoment = Time.realtimeSinceStartup;
        Invoke(nameof(Revive), reviveTime);
    }

    private void Revive()
    {
        stunned = false;
        maxHealth += 15;
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee" && stunned)
        {
            //Debug.Log("MORIDO");
            if (Time.realtimeSinceStartup - stunMoment <= reviveTime/4)
            {
                player.gameObject.GetComponent<player_combat>().healPlayer(20);
            }
            altar.GetComponent<altar_nv2>().enemyKilled();
            Destroy(this.gameObject);
        }
        else if(other.tag == "Projectile" && !stunned)
        {
            health -= 10;
        }
    }

}
