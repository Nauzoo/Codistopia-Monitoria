using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    private const string IDLE_RIGHT_1 = "Idle_right";
    private const string IDLE_LEFT_2 = "Idle_left";
    private const string IDLE_BACK_3 = "Idle_back";
    private const string IDLE_FRONT_4 = "Idle_front";

    private const string WALK_RIGHT_1 = "Walking_right";
    private const string WALK_LEFT_2 = "Walking_left";
    private const string WALK_BACK_3 = "Walking_back";
    private const string WALK_FRONT_4 = "Walking_front";

    public int speed = 3; // player speed
    private FixedJoystick moveJoystick; // Joystick class
    private Vector2 myMoveVector; // vecor x and y for current movement direction
    private Animator animator;
    private Rigidbody2D rb;

    private int lookinDir;
    

    private static Player_movement playerInsyance;

    private void Start()
    {
        animator = GetComponent<Animator>();
        moveJoystick = FixedJoystick.Instance;
        rb = GetComponent<Rigidbody2D>();        
    }
    private void Awake()
    {
        if (playerInsyance == null)
        {
            playerInsyance = this;
        }
    }
    public static Player_movement GetInstance()
    {
        return playerInsyance;
    }
    private void Update()
    {
        float x_move = 0f;
        float y_move = 0f;
        
        x_move = moveJoystick.Horizontal; // Gets a value between -1 and 1 for joystick horizontal movement 
        y_move = moveJoystick.Vertical; // Gets a value between -1 and 1 for joystick vertical movement
        
        myMoveVector = new Vector2(x_move, y_move); // movemet direction using joystick (1 for positive and -1 for negative)
        
        if (Mathf.Abs(myMoveVector[0]) >= 0.5f || Mathf.Abs(myMoveVector[1]) >= 0.5f) // Checks if joystick positions (x and y) is greater then 0.5f
        {
            if (Mathf.Abs(myMoveVector[0]) > Mathf.Abs(myMoveVector[1]))
            {
                if (myMoveVector[0] > 0.3f)
                {
                    animator.Play(WALK_RIGHT_1);
                    lookinDir = 1;
                }
                else if (myMoveVector[0] < -0.3f)
                {
                    animator.Play(WALK_LEFT_2);
                    lookinDir = 2;
                }
            }
            else if (Mathf.Abs(myMoveVector[1]) > Mathf.Abs(myMoveVector[0])) 
            {
                if (myMoveVector[1] > 0.3f)
                {
                    animator.Play(WALK_BACK_3);
                    lookinDir = 3;
                }
                else if (myMoveVector[1] < -0.3f)
                {
                    animator.Play(WALK_FRONT_4);
                    lookinDir = 4;
                }
            }

            //rb.velocity = myMoveVector * speed * Time.deltaTime;

            transform.Translate(speed * myMoveVector * Time.deltaTime); // move player's trasnform acording to myMoveVector
            
            

        }
        else
        {
            if (lookinDir == 1)
            {
                animator.Play(IDLE_RIGHT_1);
            }
            else if (lookinDir == 2)
            {
                animator.Play(IDLE_LEFT_2);
            }
            else if (lookinDir == 3)
            {
                animator.Play(IDLE_BACK_3);
            }
            else if (lookinDir == 4)
            {
                animator.Play(IDLE_FRONT_4);
            }

        }
        

    }

}
