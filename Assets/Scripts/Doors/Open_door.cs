using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Open_door : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] ChangeScene scneChanger;
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
                LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
                LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
                LastScene.lastVickPos = VickFollower.GetInstance().transform.position;
                Debug.Log(LastScene.lastVickPos);
                animator.SetTrigger("open");
                scneChanger.ChangeToScene(myDoorId);
                InteractionMannager.GetInstance().CloseInteraction(); // closes the interaction event
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
