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

    private string specialChars = "(){},;\n0123456789=";
    public class POSITION
    {
        public int Line { get; set; }
        public int Char { get; set; }
        
        public List<string> GetPosition()
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
        string myCode = text + ' ';
        string formatedCode = "";
                

        bool openString = false;
        bool openComment = false;

        string term_holder = "";
        foreach (char letter in myCode)
        {
            if (letter != ' ')
            {
                if (letter.ToString() == "'" && !openString && !openComment)
                {
                    openString = true;
                }
                else if (letter.ToString() == "'" && openString)
                {
                    openString = false;
                    formatedCode += ":string: ";
                    continue;
                }
                if (openString)
                {
                    continue;
                }
                
                else
                {
                    if (letter == '#' && !openComment)
                    {
                        openComment = true;
                    }
                    else if (letter == '\n' && openComment)
                    {
                        openComment = false;
                    }
                    
                    if (openComment)
                    {
                        continue;
                    }
                    
                    if (specialChars.Contains(letter) && formatedCode.Length > 0) {
                        formatedCode += term_holder;
                        term_holder = "";                        
                        if (formatedCode[formatedCode.Length-1] == ' ')
                        {
                            formatedCode += letter + " ";
                        }
                        else
                        {
                            formatedCode += " " + letter + " ";
                        }
                        
                    }
                    else
                    {
                        term_holder += letter;
                    }
                }
            }
            else
            {
                if (term_holder != "")
                {
                    formatedCode += term_holder + ' ';
                    term_holder = "";
                }
            }
        }
        Debug.Log(formatedCode);
        return formatedCode.Trim();
    }
    public string tokenizeCode(string code)
    {
        string tokenizedCode = "";
        string[] codeParts = code.Split(' ');

        foreach (string fragment in codeParts)
        {            
            if (fragment == "var")
            {
                tokenizedCode += "VAR_D";                
            }
            else if (fragment == "for")
            {
                tokenizedCode += "FOR_LP";
            }
            else if (fragment == "vicky.move")
            {
                tokenizedCode += "MOV";
            }
            else if (fragment == "if")
            {
                tokenizedCode += "IF_ST";
            }
            else if (fragment == "import")
            {
                tokenizedCode += "IMP";
            }            
            else if (fragment == ",")
            {
                tokenizedCode += "CO";
            }
            else if (fragment == "(")
            {
                tokenizedCode += "L_PAR";
            }
            else if (fragment == ")")
            {
                tokenizedCode += "R_PAR";
            }
            else if (fragment == ";")
            {
                tokenizedCode += "PCO";
            }
            else if (fragment == "=")
            {
                tokenizedCode += "EQ";
            }
            else if (fragment == "{")
            {
                tokenizedCode += "L_K";
            }
            else if (fragment == "}")
            {
                tokenizedCode += "R_K";
            }
            else if (fragment == "\n")
            {
                tokenizedCode += "BRK";
            }
            else
            {
                if (fragment.Length > 1 || specialChars.Contains(fragment))
                {
                    tokenizedCode += "VALUE->" + fragment;
                }
                else
                {
                    tokenizedCode += "VALUE->unknowChar" ;
                }
                
            }
            tokenizedCode += ' ';
        }
        tokenizedCode = tokenizedCode.Trim();
        Debug.Log(tokenizedCode);
        return tokenizedCode;
    }
    public void RUN_COMPARASSION(string code)
    {
        
        string formatedTypCode = formatCode(code);
        string tokCode = tokenizeCode(formatedTypCode);
        
        POSITION pos = new POSITION();

        foreach (string formatedTempCode in formatedTempCodes)
        {
            pos.Line = 0;
            pos.Char = 0;

            
            hasErros = false;

            for (int i = 0; i < formatedTempCode.Length; i++)
            {                
                if (formatedTempCode[i] == formatedTypCode[i])
                {
                    continue;
                }
                else
                {
                    hasErros = true;
                    Debug.Log(formatedTempCode[i]);
                    Debug.Log(formatedTypCode[i]);
                    break;
                }
            }
            if (!hasErros)
            {
                break;
            }
        }
        IdeEvent.showCompResult(hasErros);
        
    }
}
