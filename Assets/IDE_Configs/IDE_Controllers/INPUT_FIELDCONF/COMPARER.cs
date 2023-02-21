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

    private string specialChars = "(){},;\n/|+-*%=[]^~<>@$?!";

    private List<string> systemVariables;
    private List<string> avaybleLibraries = new List<string> { "vicky" };
    private List<string> librariesCommands = new List<string> { "vicky.move:2:int,string" };
    private List<string> importedLibraries = new List<string>();

    public class POSITION
    {
        public static int Line { get; set; }
        public static int Term { get; set; }
        
        public static List<string> GetPosition()
        {
            List<string> postion = new List<string>();
            postion.Add(Line.ToString());
            postion.Add(Term.ToString());

            return postion;
        }
    }
    public class ERROR { 
        public static void syntaxError(List<string> pos, string exp)
        {
            Debug.LogError("!Syntax Error: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void unnexpectedChar(List<string> pos, string exp)
        {
            Debug.LogError("!Unnexpected Char: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void importError(List<string> pos, string exp)
        {
            Debug.LogError("!import Error: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void unnexpectedValue(List<string> pos, string exp)
        {
            Debug.LogError("!Unnexpected Value: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void nameError(List<string> pos, string exp)
        {
            Debug.LogError("!Name Error: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void parameterOverflow(List<string> pos, string exp)
        {
            Debug.LogError("Parameter Overflow: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void unsuficientParameters(List<string> pos, string exp)
        {
            Debug.LogError("!Unsuficient Parameters: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
        public static void undefinedExpression(List<string> pos, string exp)
        {
            Debug.LogError("!Undefined Expression: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
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
                    formatedCode += "string:";
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
            Debug.Log(fragment);
            if (fragment == "var")
            {
                tokenizedCode += "VAR_D";                
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
            else if (specialChars.Contains(fragment))
            {
                tokenizedCode += fragment;
            }
            else
            {
                string token = "VALUE->";
                if (int.TryParse(fragment, out int n))
                {
                    token += "int:";
                }
                else if (fragment == "true" || fragment == "false")
                {
                    token += "bool:";
                }                
                token += fragment;
                tokenizedCode += token;

            }
            tokenizedCode += ' ';
        }
        tokenizedCode = tokenizedCode.Trim();
        Debug.Log(tokenizedCode);
        return tokenizedCode;
    }
    public void generateAST(string tokenList)
    {
        POSITION.Line = 1;
        POSITION.Term = 1;

        string[] tokenStack = tokenList.Split(' ');
        string AST = "";

        List<string> keyTokens = new List<string>{ "IMP", "VAR_D", "NAMED_VAR", "ATR_VAR", "FUNC" };
        List<string> avaibleFunctions = new List<string> { "if:1:bool", "for_interval:2:int,int" };

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
                            foreach (string command in librariesCommands)
                            {
                                if (command.Contains(lib))
                                {
                                    avaibleFunctions.Add(command);
                                    placeHolder = "";
                                }
                            }

                        }
                        else
                        {
                            ERROR.importError(POSITION.GetPosition(), $"biblioteca {lib} nao existe nos arquivos. Voce digitou o nome corretamente?");
                            break;
                        }
                    }
                }

                else if (placeHolder == "VAR_D")
                {
                    if (token.Contains("VALUE"))
                    {
                        string name = token.Split("->")[1];
                        if (!specialChars.Contains(name) && name != "string:" && !keyTokens.Contains(name))
                        {
                            placeHolder = "NAMED_VAR:" + name;
                        }
                        else
                        {
                            ERROR.nameError(POSITION.GetPosition(), "nao utilize tipos primitivos e carateres especiais para nomear variaveis!");
                            break;
                        }
                    }
                    else
                    {
                        ERROR.unnexpectedChar(POSITION.GetPosition(), "adicione um nome para a variavel, que nao seja uma palavra ou caracter reservado a linguagem!");
                        break;
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
                        ERROR.unnexpectedChar(POSITION.GetPosition(), "esperado '=', para atribuicao de valor.");
                        break;
                    }
                }
                else if (placeHolder.Contains("ATR_VAR"))
                {
                    if (token.Contains("VALUE") && (token.Contains("string") || token.Contains("int") || token.Contains("bool")))
                    {
                        string value = token.Split("->")[1];
                        if (!specialChars.Contains(value) && !keyTokens.Contains(value))
                        {
                            systemVariables.Add($"{placeHolder.Split(':')[1]}:{value}");
                            placeHolder = "";
                        }
                        else
                        {
                            ERROR.unnexpectedValue(POSITION.GetPosition(), $"'{token}', nao e valido no contexto fornecido.");
                            break;
                        }
                    }
                    else
                    {
                        ERROR.unnexpectedValue(POSITION.GetPosition(), $"'{token}', nao e valido no contexto fornecido.");
                        break;
                    }
                }

                else if (placeHolder.Contains("VALUE"))
                {                    
                    if (token == "L_PAR")
                    {
                        placeHolder = "FUNC:" + placeHolder.Split("->")[1];
                    }
                    else
                    {
                        ERROR.undefinedExpression(POSITION.GetPosition(), $"termo desconhecido '{token}'. Voce se esqueceu de importar alguma biblioteca?");
                        break;
                    }
                }

                else if (placeHolder.Contains("FUNC"))
                {
                    
                }               
                
            }
            else
            {
                if (keyTokens.Contains(token) || token.Contains("VALUE"))
                {
                    placeHolder = token;                    
                }
                else if (token == "BRK" || token == "endprogram")
                {
                    POSITION.Line++;
                    POSITION.Term = 1;
                }
                else
                {
                    ERROR.unnexpectedChar(POSITION.GetPosition(), $"caracter inesperado {token}.");
                    break;
                }
                
            }
            POSITION.Term++;
        }

        Debug.Log(AST);
        foreach (string item in avaibleFunctions)
        {
            Debug.Log(item);
        }
        foreach (string item in importedLibraries)
        {
            Debug.Log(item);
        }
        foreach (string item in systemVariables)
        {
            Debug.Log(item);
        }
        
    }
    public void RUN_COMPARASSION(string code)
    {
        systemVariables = new List<string>();
        string formatedTypCode = formatCode(code);
        string tokCode = tokenizeCode(formatedTypCode);
        generateAST(tokCode);

        /*POSITION pos = new POSITION();

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
        IdeEvent.showCompResult(hasErros);*/
        
    }
}
