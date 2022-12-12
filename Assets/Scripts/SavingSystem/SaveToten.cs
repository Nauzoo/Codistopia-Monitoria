using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveToten : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") {
            animator.SetTrigger("open"); 
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            animator.SetTrigger("close");
        }
    }
}
