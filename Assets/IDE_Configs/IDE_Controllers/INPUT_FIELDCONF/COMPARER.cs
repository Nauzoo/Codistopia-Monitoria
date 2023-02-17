using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMPARER : MonoBehaviour
{
    private string[] formatedTempCodes;
    [SerializeField] private IDEeventsMan IdeEvent;

    private bool hasErros;

    public static COMPARER Instance;
    private void Awake()
    {
        Instance = this;
    }

    public static class POSITION
    {
        public static int Line { get; set; }
        public static int Char { get; set; }
        
        public static List<string> GetPosition()
        {
            List<string> postion = new List<string>();
            postion.Add(Line.ToString());
            postion.Add(Char.ToString());

            return postion;
        }
    }
    private void Start()
    {
        if (CurrentCode.textArray != null)
        {
            formatedTempCodes = new string[CurrentCode.textArray.Length];
            for (int i = 0; i < CurrentCode.textArray.Length; i++)
            {
                formatedTempCodes[i] = formatCode(CurrentCode.textArray[i]);
            }
        }
        else {
            Debug.LogWarning("Inicialized without Temp. Code!");
        }                
        
    }
    private string formatCode(string text)
    {
        string myCode = text;
        string formatedCode = "";
                

        bool openString = false;
        foreach (char letter in myCode)
        {
            if (letter != ' ')
            {
                if (letter.ToString() == "'" && !openString)
                {
                    openString = true;
                }
                else if (letter.ToString() == "'" && openString)
                {
                    openString = false;
                    formatedCode += ":string:";
                    continue;
                }
                if (openString)
                {
                    continue;
                }
                else
                {
                    if(letter == '\n' || letter == '\t')
                    {
                        continue;
                    }
                    else
                    {
                        formatedCode += letter;
                    }
                }
            }            
        }
        Debug.Log(formatedCode);
        return formatedCode;
    }
    public void RUN_COMPARASSION(string code)
    {
        
        string formatedTypCode = formatCode(code);

        foreach (string formatedTempCode in formatedTempCodes)
        {
            int typeIncreaser = 0;
            int tempIncreaser = 0;
            hasErros = false;

            for (int i = 0; i < formatedTempCode.Length; i++)
            {
                if (!hasErros)
                {
                    continue;
                }

                if (formatedTempCode[i] == '\n' || formatedTempCode[i] == '\t')
                {
                    tempIncreaser++;
                }

                if (formatedTypCode[i] == '\n' || formatedTempCode[i] == '\t')
                {
                    if (formatedTypCode[i + tempIncreaser] == '\n')
                    {
                        POSITION.Line += 1;
                    }
                    typeIncreaser++;
                }

                if (formatedTempCode[i + tempIncreaser] == formatedTypCode[i + typeIncreaser])
                {
                    continue;
                }
                else
                {
                    hasErros = true;
                    Debug.Log(formatedTempCode[i]);
                    Debug.Log(formatedTypCode[i]);
                    Debug.Log($"line: {POSITION.GetPosition()[0]}, char: {POSITION.GetPosition()[1]}");
                    break;
                }
            }
            if (!hasErros)
            {
                continue;
            }
        }
        IdeEvent.showCompResult(hasErros);
        
    }
}
