using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    public GameObject Lucas;
    public NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, LucasLayer;

    Animator animator;

    private Vector3 destPoint;
    private bool walkpointSet;
    private bool isIdle = true;
    [SerializeField] private float range, walkSpeed = 1.0f, runSpeed = 2.5f;

    [SerializeField] float sightRange, attackRange;
    bool LucasInSight, LucasInAttackRange;

    private float idleTime = 3.0f;
    private float walkTime = 7.0f;
    private float timeSinceLastTransition;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Lucas = GameObject.Find("Lucas");
        animator = GetComponent<Animator>();
        timeSinceLastTransition = Time.time;
        agent.isStopped = true; // Zaustavi agenta na početku
        animator.SetBool("Walk", false);
        animator.SetFloat("Run", 0.0f);
    }

    void Update()
    {
        LucasInSight = Physics.CheckSphere(transform.position, sightRange, LucasLayer);
        LucasInAttackRange = Physics.CheckSphere(transform.position, attackRange, LucasLayer);

        if (isAttacking)
        {
            // Ako napada, zaustavi agenta
            agent.isStopped = true;
        }
        else
        {
            if (!LucasInSight && !LucasInAttackRange)
            {
                Patrol();
            }
            else if (LucasInSight && !LucasInAttackRange)
            {
                Chase(runSpeed); // Promijenjeno na runSpeed jer je trčanje kada vidi igrača
            }
            else if (LucasInSight && LucasInAttackRange)
            {
                Attack();
            }

            ManageAnimations();
        }
    }

    void ManageAnimations()
    {
        float currentTime = Time.time;
        if (!LucasInSight)
        {
            if (isIdle)
            {
                if (currentTime - timeSinceLastTransition > idleTime)
                {
                    isIdle = false;
                    timeSinceLastTransition = currentTime;
                    animator.SetBool("Walk", true);
                    agent.isStopped = false;
                    agent.speed = walkSpeed; // Set agent speed to walk speed when walking
                }
            }
            else
            {
                if (currentTime - timeSinceLastTransition > walkTime)
                {
                    isIdle = true;
                    timeSinceLastTransition = currentTime;
                    animator.SetBool("Walk", false);
                    agent.isStopped = true; // Stop the agent during idle
                }
            }
        }
        else
        {
            timeSinceLastTransition = Time.time;
            isIdle = false;
        }
    }

    void Chase(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
        animator.SetBool("Walk", false);
        animator.SetFloat("Run", 1.0f);
        agent.SetDestination(Lucas.transform.position);
        RotateTowards(Lucas.transform);
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
        }
    }

    void OnAttackAnimationEnd() // Ovu funkciju treba pozvati na kraju animacije napada
    {
        isAttacking = false;
        agent.isStopped = false;
        // Resetiraj animacije
        animator.ResetTrigger("Attack");
    }

    void Patrol()
    {
        if (!isIdle)
        {
            if (!walkpointSet) SearchForDest();
            if (walkpointSet)
            {
                agent.SetDestination(destPoint);
                if (Vector3.Distance(transform.position, destPoint) < 10)
                {
                    walkpointSet = false;
                }
            }
        }
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }

    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
