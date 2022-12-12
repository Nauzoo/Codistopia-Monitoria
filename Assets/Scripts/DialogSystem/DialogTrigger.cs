using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DialogTrigger : MonoBehaviour
{
    [Header("Interaction Sign")]
    [SerializeField] private GameObject visualCue; // the litle exclamation point on top of the npcs
    public TextAsset dialogFile; // the dialog file of the object

    private bool playerInRange; // variable that keeps if the player is in range of the object
    
    private void Awake()
    {
        playerInRange = false; 
        visualCue.SetActive(false); // makes the exclamation poit invisible by default
    }

    private void Update()
    {
        if (playerInRange && !DialogMannagerV2.GetInstance().playingDialog) // checks if the player is in range and there's no dialog playing
        {
            visualCue.SetActive(true);

            List<string> myStory = new List<string>(dialogFile.text.Split("\n")).ToList();
            if (InteractionMannager.GetInstance().Interaction()) { // checks for the interaction button input 
                DialogMannagerV2.GetInstance().TriggerDialog(myStory); // triggers the dialog system box
                InteractionMannager.GetInstance().CloseInteraction(); // closes the interaction event
            }
        }

        else {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider) // checks for player entering in range
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) // checks for player leaving the range
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
