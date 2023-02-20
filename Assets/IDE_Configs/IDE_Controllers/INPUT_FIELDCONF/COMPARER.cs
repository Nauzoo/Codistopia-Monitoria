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

    private string specialChars = "(){},;\n/|+-*%=[]^~<>@$¨?!";

    private List<string> systemVariables;
    private List<string> avaybleLibraries = new List<string> { "vicky" };    
    private List<string> importedLibraries = new List<string>();

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
        systemVariables = new List<string>();
        
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
        codeParts[codeParts.Length-1] = "//";

        foreach (string fragment in codeParts)
        {
            Debug.Log(fragment);
            if (fragment == "var")
            {
                tokenizedCode += "VAR_D";                
            }
            else if (fragment == "for")
            {
                tokenizedCode += "FOR_LP";
            }
            /*else if (fragment == "vicky.move")
            {
                tokenizedCode += "MOV";
            }*/
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
            else if (fragment == "//")
            {
                tokenizedCode += "endprogram";
            }
            else
            {
                tokenizedCode += "VALUE->" + fragment;

            }
            tokenizedCode += ' ';
        }
        tokenizedCode = tokenizedCode.Trim();
        Debug.Log(tokenizedCode);
        return tokenizedCode;
    }
    public void generateAST(string tokenList)
    {
        string[] tokenStack = tokenList.Split(' ');
        string AST = "";

        List<string> keyTokens = new List<string>{ "IMP", "VAR_D", "NAMED_VAR", "ATR_VAR", "IF_ST", "FOR_LP", "MOV", "FUNC" };
        List<string> avaibleFunctions = new List<string> { "IF_ST", "FOR_LP", "MOV" };

        string placeHolder = "";        

        for (int i = 0; i < tokenStack.Length; i++)
        {
            string token = tokenStack[i]; 

            if (placeHolder != "")
            {
                if (placeHolder == "IMP")
                {
                    if (token.Contains("VALUE"))
                    {
                        string lib = token.Split("->")[1];
                        if (avaybleLibraries.Contains(lib))
                        {
                            importedLibraries.Add(lib);                            
                        }
                        else
                        {
                            // add import error later
                            break;
                        }
                    }
                }

                else if (placeHolder == "VAR_D")
                {
                    if (token.Contains("VALUE"))
                    {
                        string name = token.Split("->")[1];
                        if (!specialChars.Contains(name) && name != ":string:" && !keyTokens.Contains(name))
                        {
                            placeHolder = "NAMED_VAR:" + name;
                        }
                        else
                        {
                            // add var name error later
                        }
                    }
                    else
                    {
                        // add error later
                    }
                }
                else if (placeHolder.Contains("NAMED_VAR"))
                {
                    if (token == "EQ")
                    {
                        placeHolder = "ATR_VAR:" + placeHolder.Split(':')[1];   
                    }
                    else
                    {
                        // add var atribuition error later
                    }
                }
                else if (placeHolder.Contains("ATR_VAR"))
                {
                    if (token.Contains("VALUE"))
                    {
                        string value = token.Split("->")[1];
                        if (!specialChars.Contains(value) && !keyTokens.Contains(value))
                        {
                            systemVariables.Add($"{placeHolder.Split(':')[1]}:{value}");
                        }
                        else
                        {
                            // add var atribuition error later
                        }
                    }
                    else
                    {
                        // add var atribuition error later;
                    }
                }

                else if (placeHolder.Contains("VALUE"))
                {
                    if (token == "L_PAR")
                    {
                        placeHolder = "FUNC:" + token.Split("->")[1];
                    }
                }

                else if (placeHolder.Contains("FUNC"))
                {
                    
                }
            }
            else
            {
                if (token == "PCO")
                {
                    placeHolder = "";
                }

                else if (keyTokens.Contains(token) || keyTokens.Contains("VALUE"))
                {
                    placeHolder = token;
                    continue;
                }
                
            }
        }
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
