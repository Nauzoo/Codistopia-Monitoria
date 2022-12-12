using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeSetter : MonoBehaviour
{
    [SerializeField] private string myCode;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            CurrentCode.text = myCode;
        }
    }
}
