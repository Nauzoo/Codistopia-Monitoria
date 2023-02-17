using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CodeSetter : MonoBehaviour
{
    [SerializeField] private TextAsset myCode;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            string[] codeList = myCode.text.Split("##");
            CurrentCode.textArray = codeList;
        }
    }
}
