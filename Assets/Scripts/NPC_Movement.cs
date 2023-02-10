using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public bool isStatic;

    private const string IDLE_RIGHT_1 = "Idle_right";
    private const string IDLE_LEFT_2 = "Idle_left";
    private const string IDLE_BACK_3 = "Idle_back";
    private const string IDLE_FRONT_4 = "Idle_front";

    private const string WALK_RIGHT_1 = "Walking_right";
    private const string WALK_LEFT_2 = "Walking_left";
    private const string WALK_BACK_3 = "Walking_back";
    private const string WALK_FRONT_4 = "Walking_front";

    [SerializeField] private Transform[] mPositions;
    [SerializeField] private int speed;
    [SerializeField] private float waitTime;

    public bool canMove;

    private float timeHolder;
    private int CurMovePos;

    private float deltaXmove;
    private float deltaYmove;
    private float lastX = 0;
    private float lastY = 0;

    public Animator animator;

    private int lookinDir = 4;

    void Start()
    {
        RandNewPos();
        animator = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        if (!isStatic && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, mPositions[CurMovePos].position, speed * Time.deltaTime);

            float tarDis = Vector2.Distance(transform.position, mPositions[CurMovePos].position);

            if (tarDis <= .2f)
            {
                switch (lookinDir)
                {
                    case 1:
                        animator.Play(IDLE_RIGHT_1);
                        break;
                    case 2:
                        animator.Play(IDLE_LEFT_2);
                        break;
                    case 3:
                        animator.Play(IDLE_BACK_3);
                        break;
                    case 4:
                        animator.Play(IDLE_FRONT_4);
                        break;
                }

                if (timeHolder <= 0)
                {
                    RandNewPos();
                }
                else
                {
                    timeHolder -= Time.deltaTime;
                    
                }
            }
            else
            {
                deltaXmove = transform.position.x - lastX;
                deltaYmove = transform.position.y - lastY;

                if (Mathf.Abs(deltaXmove) > Mathf.Abs(deltaYmove))
                {
                    if (lastX > transform.position.x)
                    {
                        animator.Play(WALK_LEFT_2);
                        lookinDir = 2;
                    }
                    else if (lastX < transform.position.x)
                    {
                        animator.Play(WALK_RIGHT_1);
                        lookinDir = 1;
                    }
                }
                else if (Mathf.Abs(deltaXmove) < Mathf.Abs(deltaYmove))
                {
                    if (lastY > transform.position.y)
                    {
                        animator.Play(WALK_FRONT_4);
                        lookinDir = 4;
                    }
                    else if (lastY < transform.position.y)
                    {                        
                        animator.Play(WALK_BACK_3);
                        lookinDir = 3;
                    }
                }

                lastX = transform.position.x;
                lastY = transform.position.y;
            }
        }
        else
        {
            if (!canMove)
            {
                float meXplayerDx = transform.position.x - Player_movement.GetInstance().transform.position.x;
                float meXplayerDy = transform.position.y - Player_movement.GetInstance().transform.position.y;
               
                if (Mathf.Abs(meXplayerDx) > Mathf.Abs(meXplayerDy))
                {
                    if (transform.position.x > Player_movement.GetInstance().transform.position.x)
                    {
                        animator.Play(IDLE_LEFT_2);
                    }
                    else if (transform.position.x < Player_movement.GetInstance().transform.position.x)
                    {
                        animator.Play(IDLE_RIGHT_1);
                    }
                }
                else if (Mathf.Abs(meXplayerDy) > Mathf.Abs(meXplayerDx))
                {
                    if (transform.position.y > Player_movement.GetInstance().transform.position.y)
                    {
                        animator.Play(IDLE_FRONT_4);
                    }
                    else if (transform.position.y < Player_movement.GetInstance().transform.position.y)
                    {
                        animator.Play(IDLE_BACK_3);
                    }
                }                
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Vicky")
        {
            StartCoroutine(changeMoveState(false, 0));           
        }
    
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Vicky")
        {
            StartCoroutine(changeMoveState(true, 2));
        }
    }
    private void RandNewPos()
    {
        CurMovePos = Random.Range(0, mPositions.Length);
        timeHolder = waitTime;
    }
    IEnumerator changeMoveState (bool state, int delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = state;
    }
}
