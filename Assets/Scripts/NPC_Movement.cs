using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public bool isStatic;

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

    private int animState = 0;
    private int lookDir = 0;
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
                animState = 0;
                animator.SetInteger("animIndex", lookDir + animState);
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
                animState = 4;
                deltaXmove = transform.position.x - lastX;
                deltaYmove = transform.position.y - lastY;

                if (Mathf.Abs(deltaXmove) > Mathf.Abs(deltaYmove))
                {
                    if (lastX > transform.position.x)
                    {
                        lookDir = 0;
                    }
                    else if (lastX < transform.position.x)
                    {
                        lookDir = 1;
                    }
                }
                else if (Mathf.Abs(deltaXmove) < Mathf.Abs(deltaYmove))
                {
                    if (lastY > transform.position.y)
                    {
                        lookDir = 2;
                    }
                    else if (lastY < transform.position.y)
                    {
                        lookDir = 3;
                    }
                }
                animator.SetInteger("animIndex", lookDir + animState);

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
                        lookDir = 0;
                    }
                    else if (transform.position.x < Player_movement.GetInstance().transform.position.x)
                    {
                        lookDir = 1;
                    }
                }
                else if (Mathf.Abs(meXplayerDy) > Mathf.Abs(meXplayerDx))
                {
                    if (transform.position.y > Player_movement.GetInstance().transform.position.y)
                    {
                        lookDir = 2;
                    }
                    else if (transform.position.y < Player_movement.GetInstance().transform.position.y)
                    {
                        lookDir = 3;
                    }
                }
                animator.SetInteger("animIndex", lookDir + animState);
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
        if (!state)
        {
            animState = 0;
        }
    }
}
