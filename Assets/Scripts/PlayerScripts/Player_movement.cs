using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public int speed = 3; // player speed
    public FixedJoystick moveJoystick; // Joystick class
    private Vector2 myMoveVector; // vecor x and y for current movement direction
    private Animator animator;
    private int walking_anim_i = 1;
    private int stoped_anim_i = 0;

    private static Player_movement playerInsyance;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
                    walking_anim_i = 3;
                }
                else if (myMoveVector[0] < -0.3f)
                {
                    walking_anim_i = 4;
                }
            }
            else if (Mathf.Abs(myMoveVector[1]) > Mathf.Abs(myMoveVector[0])) 
            {
                if (myMoveVector[1] > 0.3f)
                {
                    walking_anim_i = 2;
                }
                else if (myMoveVector[1] < -0.3f)
                {
                    walking_anim_i = 1;
                }
            }            
                        
            transform.Translate(speed * myMoveVector * Time.deltaTime); // move player's trasnform acording to myMoveVector

            stoped_anim_i = 0;

        }
        else
        {
            stoped_anim_i = walking_anim_i + 4;
            
        }
        
        if (stoped_anim_i == 0)
        {
            animator.SetInteger("Anim_index", walking_anim_i);            
            
        }
        else
        {
            animator.SetInteger("Anim_index", stoped_anim_i);
            
        }

    }       

    
}
