using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TutorialSetter : MonoBehaviour
{
    [SerializeField] private TextAsset myTut;
    [SerializeField] private TextAsset myTips;

    private void OnTriggerEnter2D(Collider2D collider)
    {        
        if (collider.gameObject.tag == "Player")
        {
            CurrentTutorial.text = myTut.text;
            TipsForCode.tips = myTips.text;
        }
    }
}
