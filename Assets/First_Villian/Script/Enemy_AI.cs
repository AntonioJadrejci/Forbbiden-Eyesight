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
    [SerializeField] private float range, walkSpeed = 1.0f, runSpeed = 2.5f;

    [SerializeField] float sightRange, attackRange;
    bool LucasInSight, LucasInAttackRange;

    private float idleTime = 5.0f;
    private float walkTime = 7.0f;
    private float timeSinceLastTransition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Lucas = GameObject.Find("Lucas");
        animator = GetComponent<Animator>();
        timeSinceLastTransition = Time.time;
        animator.SetBool("Walk", false);
        animator.SetFloat("Run", 0.0f);
    }

    void Update()
    {
        LucasInSight = Physics.CheckSphere(transform.position, sightRange, LucasLayer);
        LucasInAttackRange = Physics.CheckSphere(transform.position, attackRange, LucasLayer);

        if (!LucasInSight && !LucasInAttackRange)
        {
            Patrol();
        }
        else if (LucasInSight && !LucasInAttackRange)
        {
            Chase(walkSpeed);
        }
        else if (LucasInSight && LucasInAttackRange)
        {
            Chase(runSpeed);
        }

        if (LucasInAttackRange)
        {
            Attack();
        }

        ManageAnimations();
    }

    void ManageAnimations()
    {
        float currentTime = Time.time;
        if (!LucasInSight)
        {
            if (currentTime - timeSinceLastTransition > walkTime + idleTime)
            {
                timeSinceLastTransition = currentTime;
                animator.SetBool("Walk", true);
                animator.SetFloat("Run", 0.0f);
            }
            else if (currentTime - timeSinceLastTransition > walkTime)
            {
                animator.SetBool("Walk", false);
                animator.SetFloat("Run", 0.0f);
            }
        }
        else
        {
            timeSinceLastTransition = Time.time - walkTime;
        }
    }

    void Chase(float speed)
    {
        agent.speed = speed;
        animator.SetBool("Walk", speed == walkSpeed);
        animator.SetFloat("Run", speed == runSpeed ? 1.0f : 0.0f);
        agent.SetDestination(Lucas.transform.position);
        RotateTowards(Lucas.transform);
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
        }
    }

    void Patrol()
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
