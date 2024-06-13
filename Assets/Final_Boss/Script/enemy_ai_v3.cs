using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_ai_v3 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float chaseSpeed = 8f;
    [SerializeField] float sightDistance = 10f;
    [SerializeField] float attackRange = 7f; // Increased attack range for ability
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float patrolRadius = 10f; // Patrol radius for random points
    [SerializeField] float minIdleTime = 3f; // Minimum idle time
    [SerializeField] float maxIdleTime = 4f; // Maximum idle time

    [SerializeField] AudioClip idleSound;
    [SerializeField] AudioClip patrolSound;
    [SerializeField] AudioClip chaseSound;
    [SerializeField] AudioClip attackSound;
    AudioSource audioSource;

    float idleTimer = 0;
    float distanceToTarget = Mathf.Infinity;
    float currentIdleTime;

    NavMeshAgent navMeshAgent;
    Animator animator;

    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetRandomIdleTime();
        SetRandomDestination();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        Debug.Log("Current State: " + currentState); // Debug log for current state

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrol:
                HandlePatrolState();
                break;
            case EnemyState.Chase:
                HandleChaseState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
        }
    }

    void HandleIdleState()
    {
        Debug.Log("State: Idle"); // Debug log for Idle state
        idleTimer += Time.deltaTime;
        animator.SetBool("Walking", false);
        PlaySound(idleSound);

        if (idleTimer >= currentIdleTime)
        {
            SetRandomDestination();
            currentState = EnemyState.Patrol;
        }

        CheckPlayerDetection();
    }

    void HandlePatrolState()
    {
        Debug.Log("State: Patrol"); // Debug log for Patrol state
        animator.SetBool("Walking", true);
        PlaySound(patrolSound);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetRandomIdleTime();
            currentState = EnemyState.Idle;
        }

        CheckPlayerDetection();
    }

    void HandleChaseState()
    {
        Debug.Log("State: Chase"); // Debug log for Chase state
        animator.SetBool("Walking", true);
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(target.position);
        FaceTarget();
        PlaySound(chaseSound);

        if (distanceToTarget > sightDistance)
        {
            SetRandomDestination();
            currentState = EnemyState.Patrol;
        }

        if (distanceToTarget <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    void HandleAttackState()
    {
        Debug.Log("State: Attack"); // Debug log for Attack state
        animator.SetBool("Walking", false);
        animator.SetTrigger("Attack");
        AttackTarget();
        PlaySound(attackSound);

        if (distanceToTarget > attackRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    void AttackTarget()
    {
        FaceTarget();
        Debug.Log("Attacking Target"); // Debug log for attacking
        if (distanceToTarget <= attackRange)
        {
            navMeshAgent.velocity = Vector3.zero;
        }
        else
        {
            currentState = EnemyState.Chase;
        }
    }

    void CheckPlayerDetection()
    {
        RaycastHit hit;
        Vector3 playerDirection = target.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Lucas"))
            {
                Debug.Log("Player Detected: " + hit.collider.name); // Debug log for player detection
                currentState = EnemyState.Chase;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        Debug.Log("Facing Target"); // Debug log for facing target
    }

    void PlaySound(AudioClip soundClip)
    {
        if (audioSource != null && (!audioSource.isPlaying || audioSource.clip != soundClip))
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(navHit.position);
            navMeshAgent.speed = walkSpeed;
            Debug.Log("New Patrol Destination: " + navHit.position); // Debug log for new patrol destination
        }
        else
        {
            Debug.LogWarning("Failed to find a valid NavMesh position for random patrol.");
        }
    }

    void SetRandomIdleTime()
    {
        currentIdleTime = Random.Range(minIdleTime, maxIdleTime);
        idleTimer = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
