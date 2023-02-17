using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Indexer : MonoBehaviour
{
    [SerializeField] TMP_InputField TextInput;

    private void Start()
    {
        Invoke("OnCodeChange", 1);
    }
    // Update is called once per frame
    public void OnCodeChange()
    {
        string[] codeTxt = TextInput.text.Split(" ");

        string formatedCode = "";

        foreach(string term in codeTxt)
        {
            string w = term.Trim();

            if (w == "if")
            {
                formatedCode += $"<color=blue>{w}</color> ";
            }
            else if (w == "var")
            {
                formatedCode += $"<color=red>{w}</color> ";
            }

            else
            {
                formatedCode += $"{w} ";
            }
        }

        TextInput.text = formatedCode;

    }
}
