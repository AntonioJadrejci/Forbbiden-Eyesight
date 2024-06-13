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
    [SerializeField] float attackRange = 7f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float patrolRadius = 10f;
    [SerializeField] float minIdleTime = 3f;
    [SerializeField] float maxIdleTime = 4f;

    [SerializeField] AudioClip idleSound;
    [SerializeField] AudioClip patrolSound;
    [SerializeField] AudioClip chaseSound;
    [SerializeField] AudioClip attackSound;

    float idleTimer = 0;
    float distanceToTarget = Mathf.Infinity;
    float currentIdleTime;

    NavMeshAgent navMeshAgent;
    Animator animator;
    AudioSource audioSource;

    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (target == null)
        {
            Debug.LogError("Target is not assigned. Please assign a target Transform in the Inspector.");
        }
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned. Please add a NavMeshAgent component to the GameObject.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned. Please add an Animator component to the GameObject.");
        }
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned. Please add an AudioSource component to the GameObject if you want to play sounds.");
        }

        SetRandomIdleTime();
        SetRandomDestination();
    }

    void Update()
    {
        if (target == null || navMeshAgent == null || animator == null)
        {
            Debug.LogError("One or more required components are missing.");
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

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
        idleTimer += Time.deltaTime;
        if (idleTimer >= currentIdleTime)
        {
            SetRandomDestination();
            currentState = EnemyState.Patrol;
        }
        animator.SetBool("Walking", false);
        PlaySound(idleSound);
        CheckPlayerDetection();
    }

    void HandlePatrolState()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetRandomIdleTime();
            currentState = EnemyState.Idle;
        }
        animator.SetBool("Walking", true);
        PlaySound(patrolSound);
        CheckPlayerDetection();
    }

    void HandleChaseState()
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(target.position);
        if (distanceToTarget > sightDistance)
        {
            SetRandomDestination();
            currentState = EnemyState.Patrol;
        }
        if (distanceToTarget <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        animator.SetBool("Walking", true);
        PlaySound(chaseSound);
        FaceTarget();
    }

    void HandleAttackState()
    {
        if (distanceToTarget > attackRange)
        {
            currentState = EnemyState.Chase;
        }
        animator.SetBool("Walking", false);
        animator.SetTrigger("Attack");
        PlaySound(attackSound);
        navMeshAgent.SetDestination(transform.position); // Stop moving
        FaceTarget();
    }

    void CheckPlayerDetection()
    {
        RaycastHit hit;
        Vector3 playerDirection = target.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Lucas"))
            {
                currentState = EnemyState.Chase;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
