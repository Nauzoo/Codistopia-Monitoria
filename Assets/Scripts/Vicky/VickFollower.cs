using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VickFollower : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Transform moveTarget;
    private Animator animator;

    private float lastX = 0;
    private float lastY = 0;

    private float deltaXmove;
    private float deltaYmove;

    private NavMeshAgent agent;

    private static VickFollower vickyInstance;

    private void Awake()
    {
        if (vickyInstance == null)
        {
            vickyInstance = this;
        }
    }

    public static VickFollower GetInstance()
    {
        return vickyInstance;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Update()
    {
        agent.SetDestination(moveTarget.position);
        deltaXmove = transform.position.x - lastX;
        deltaYmove = transform.position.y - lastY;

        if (Mathf.Abs(deltaXmove) > Mathf.Abs(deltaYmove))
        {
            if (lastX > transform.position.x)
            {
                animator.SetTrigger("lookLeft");
            }
            else if (lastX < transform.position.x)
            {
                animator.SetTrigger("lookRight");
            }
        }
        else if (Mathf.Abs(deltaXmove) < Mathf.Abs(deltaYmove))
        {
            if (lastY > transform.position.y)
            {
                animator.SetTrigger("lookDown");
            }
            else if (lastY < transform.position.y)
            {
                animator.SetTrigger("lookUp");
            }
        }

        lastX = transform.position.x;
        lastY = transform.position.y;
    }

}
