using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class t800_soul : MonoBehaviour
{

    public NavMeshAgent t800;

    public Transform player;

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
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("Capsule").transform;
        t800 = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        else if (!playerInAttackRange) ChasePlayer();
        else AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) FindWalkPoint();
        else t800.SetDestination(walkPoint);

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
        t800.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        t800.SetDestination(player.position);

        transform.LookAt(player.position);

        if (!alreadyAttacked)
        {
            //Código para el ataque (sería similar al de disparar)

            alreadyAttacked = true;
            Invoke(nameof(AttackReset), timeBetweenAttacks);
        }
    }
    private void AttackReset()
    {
        alreadyAttacked = false;
    }

}
