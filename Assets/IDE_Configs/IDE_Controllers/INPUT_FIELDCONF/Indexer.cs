using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Indexer : MonoBehaviour
{
    [SerializeField] TMP_InputField TextInput;
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {        
        Debug.Log(text.text);
        Invoke("OnCodeChange", 1);
    }
    // Update is called once per frame
    public void OnCodeChange()
    {
        string[] codeTxt = text.text.Split(" ");

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

        text.text = formatedCode;

    }
}
