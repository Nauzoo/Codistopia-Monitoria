using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCScreenChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pcScreen;
    [SerializeField] private Sprite myScreen;
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") {
            pcScreen.sprite = myScreen;
        }
    }

}
