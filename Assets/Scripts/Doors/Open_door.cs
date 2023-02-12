using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Open_door : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    private Animator animator;

    [SerializeField] int myDoorId;
    private void Start()
    {
        visualCue.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player"){
            if (InteractionMannager.GetInstance().Interaction())
            { // checks for the interaction button input 
                InteractionMannager.GetInstance().CloseInteraction(); // closes the interaction event
                LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
                LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
                LastScene.lastVickPos = VickFollower.GetInstance().transform.position;
                animator.SetTrigger("open");
                ChangeScene.Instance.ChangeToScene(myDoorId);
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        visualCue.SetActive(false);
    }
}
