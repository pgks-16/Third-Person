using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    public Transform[] targetPoint;
    public int currentPoint;

    public NavMeshAgent agent;

    public Animator Animator;

    public float waitAtPoint = 2f;
    [SerializeField]private float waitCounter;

    public enum AIState
    {
        isDead, isSeekTargetPoint, isSeekPlayer, isAttack
    }
    public AIState state;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position,PlayerController.instance.transform.position);

        if (!PlayerController.instance.isDead) 
        { 
            if (distanceToPlayer >= 1f && distanceToPlayer <= 8f && !CheckWinner.instance.isWinner) 
            {
                state = AIState.isSeekPlayer;
            }
            else if (distanceToPlayer > 8)
            {
                state = AIState.isSeekTargetPoint;
            }
            else if (CheckWinner.instance.isWinner)
            {
                state = AIState.isSeekTargetPoint;
            }
            else
            {
                state = AIState.isAttack;
            }
        }
        else
        {
            state = AIState.isSeekTargetPoint;
            Animator.SetBool("Attack", false);
            Animator.SetBool("Run", true);
        }


        switch (state)
        {
            case AIState.isDead:
                break;

            case AIState.isSeekPlayer:
                agent.SetDestination(PlayerController.instance.transform.position);
                Animator.SetBool("Run", true);
                Animator.SetBool("Attack", false);
                break;
            case AIState.isSeekTargetPoint:
                agent.SetDestination(targetPoint[currentPoint].position);
                if (agent.remainingDistance <= .2f)
                {
                    if (waitCounter > 0)
                    {
                        waitCounter -= Time.deltaTime;
                        Animator.SetBool("Run", false);
                    }
                    else
                    {
                        currentPoint++;
                        waitCounter = waitAtPoint;
                        Animator.SetBool("Run", true);
                    }

                    if (currentPoint >= targetPoint.Length)
                    {
                        currentPoint = 0;
                    }
                    agent.SetDestination(targetPoint[currentPoint].position);
                }
                break;

            case AIState.isAttack:
                RotateTowardsPlayer();
                agent.stoppingDistance = 1;
                Animator.SetBool("Attack", true);
                Animator.SetBool("Run", false);
                break;

        }

    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (PlayerController.instance.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
