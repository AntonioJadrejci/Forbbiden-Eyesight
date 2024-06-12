using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    public GameObject Lucas; // Target player
    public NavMeshAgent agent; // Navigation agent

    [SerializeField] LayerMask whatIsGround, whatIsPlayer; // Detection layers

    Animator animator; // Animator for controlling animations

    private Vector3 walkPoint; // Random walk point
    private bool walkPointSet;
    private float walkPointRange = 10f; // Range for random walk

    [SerializeField] private float walkSpeed = 1.0f, runSpeed = 2.5f; // Movement speeds
    [SerializeField] private float sightRange, attackRange; // Detection ranges
    private bool playerInSightRange, playerInAttackRange;

    private bool isIdle = true;
    private float idleTime = 3.0f, walkTime = 5.0f; // Times for state transitions
    private float timeSinceLastTransition; // Timer tracking

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.isStopped = true;
        timeSinceLastTransition = Time.time;

        // Initialize layer masks
        whatIsGround = LayerMask.GetMask("Nasmesh");
        whatIsPlayer = LayerMask.GetMask("Lucas");

        if (Lucas == null)
        {
            Debug.LogError("Lucas GameObject is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patrol();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange && playerInSightRange)
            AttackPlayer();

        ManageAnimations();
    }

    void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }

        TransitionBetweenStates();
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        if (!isAttacking)
        {
            agent.speed = runSpeed;
            agent.isStopped = false;
            agent.SetDestination(Lucas.transform.position);

            // Ensure the agent is not stuck
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }
        }
    }

    void AttackPlayer()
    {
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, Lucas.transform.position) <= attackRange)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(Lucas.transform);

            if (!isAttacking)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else
        {
            ChasePlayer();
        }
    }

    void ManageAnimations()
    {
        animator.SetBool("Walk", agent.velocity.magnitude > 0.1f && !isAttacking);
        animator.SetFloat("Run", ((agent.speed == runSpeed) && (agent.velocity.magnitude > 0.1f)) ? 1.0f : 0.0f);
    }

    void TransitionBetweenStates()
    {
        float timePassed = Time.time - timeSinceLastTransition;

        if (isIdle)
        {
            if (timePassed > idleTime)
            {
                isIdle = false;
                agent.isStopped = false;
                agent.speed = walkSpeed;
                timeSinceLastTransition = Time.time;
            }
        }
        else
        {
            if (timePassed > walkTime || agent.remainingDistance < 1f)
            {
                isIdle = true;
                agent.isStopped = true;
                timeSinceLastTransition = Time.time;
            }
        }
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;

        // Ensure that the NavMeshAgent resumes properly
        agent.isStopped = false;
        if (playerInSightRange && !playerInAttackRange)
        {
            agent.speed = runSpeed;
            ChasePlayer();
        }
        else if (!playerInSightRange && !playerInAttackRange)
        {
            agent.speed = walkSpeed;
            Patrol();
        }
    }
}
