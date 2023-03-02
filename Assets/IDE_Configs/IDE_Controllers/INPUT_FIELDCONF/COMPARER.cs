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
        public static void undefinedValue(List<string> pos, string exp)
        {
            Debug.LogError("!Undefined Value: " + exp + " Linha: " + pos[0] + ", Termo: " + pos[1]);
        }
    }

    private class NUM_NODE
    {
        int n;
        public NUM_NODE(int number)
        {
            n = number;
        }
    }
    private class BYNARY_OP
    {
        public string op;
        public ArrayList terms = new ArrayList();

    }
    private class VALUE_NODE {
        string v;
        public VALUE_NODE(string value) {
            v = value;
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

        string strHolder = "";

        string term_holder = "";
        foreach (char letter in myCode)
        {
            if (letter != ' ')
            {
                if (letter.ToString() == "'" && !openString && !openComment)
                {
                    openString = true;
                    strHolder += letter;
                    continue;
                }
                else if (letter.ToString() == "'" && openString)
                {
                    openString = false;
                    strHolder += letter;
                    formatedCode += strHolder;
                    strHolder = "";
                    continue;
                }
                if (openString)
                {
                    strHolder += letter;
                    continue;
                }
                
                else
                {
                    if (letter == '#' && !openComment)
                    {
                        openComment = true;
                        continue;
                    }
                    else if (letter == '\n' && openComment)
                    {                        
                        openComment = false;                    
                        
                    }
                    
                    if (openComment)
                    {
                        continue;
                    }
                    
                    if (specialChars.Contains(letter)) {
                        formatedCode += term_holder;
                        term_holder = "";                        
                        if (formatedCode.Length > 0 && formatedCode[formatedCode.Length-1] == ' ')
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
                if (openString)
                {
                    strHolder += "|spa|";
                }
            }            
        }
        Debug.Log(formatedCode);
        return formatedCode + " /end";
            //.Trim((char)8203);
    }
    public ArrayList NodeTerms(List<string> exp)
    {
        ArrayList NodeList = new ArrayList();
        foreach (string term in exp)
        {
            if (term.Contains("INT") && !term.Contains("VALUE")) NodeList.Add(new NUM_NODE(int.Parse(term)));
            
            else if (term.Contains("VALUE")) NodeList.Add(new VALUE_NODE(term));
            
            else NodeList.Add(term);
        }

        return NodeList;
    }
    public ArrayList Addterms(ArrayList exp)
    {
        ArrayList newExp = new ArrayList();
        for (int i = 0; i < exp.Count; i++)
        {
            var n = exp[i];
            if (n.ToString() == "ADD_OP")
            {
                if (newExp.Count > 0 && i < exp.Count - 1)
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "ADD";
                    op.terms.Add(newExp[newExp.Count - 1]);
                    op.terms.Add(exp[i + 1]);

                    newExp[newExp.Count - 1] = op;
                    i++;
                    continue;
                }
                else
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "ADD";
                    op.terms.Add("null");
                    op.terms.Add("null");

                    newExp[newExp.Count - 1] = op;
                }
            }
            else if (n.ToString() == "SUB_OP")
            {
                if (newExp.Count > 0 && i < exp.Count - 1)
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "SUB";
                    op.terms.Add(newExp[newExp.Count - 1]);
                    op.terms.Add(exp[i + 1]);

                    newExp[newExp.Count - 1] = op;
                    i++;
                    continue;
                }
                else
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "SUB";
                    op.terms.Add("null");
                    op.terms.Add("null");

                    newExp[newExp.Count - 1] = op;
                }
            }
            else
            {
                newExp.Add(n);
            }
        }
        return newExp;
    }
    public ArrayList MutTerms(List<string> exp)
    {
        ArrayList newExp = new ArrayList();

        for (int i = 0; i < exp.Count; i ++)
        {
            var n = exp[i];
            if (n.ToString() == "MUL_OP")
            {
                if (newExp.Count > 0 && i < exp.Count - 1)
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "MUL";
                    op.terms.Add(newExp[newExp.Count - 1]);
                    op.terms.Add(exp[i + 1]);

                    newExp[newExp.Count - 1] = op;
                    i++;
                    continue;
                }
                else
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "MUL";
                    op.terms.Add("null");
                    op.terms.Add("null");

                    newExp[newExp.Count - 1] = op;
                }
            }
            else if (n.ToString() == "DIV_OP")
            {
                if (newExp.Count > 0 && i < exp.Count - 1)
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "DIV";
                    op.terms.Add(newExp[newExp.Count - 1]);
                    op.terms.Add(exp[i + 1]);

                    newExp[newExp.Count - 1] = op;
                    i++;
                    continue;
                }
                else
                {
                    BYNARY_OP op = new BYNARY_OP();
                    op.op = "DIV";
                    op.terms.Add("null");
                    op.terms.Add("null");

                    newExp[newExp.Count - 1] = op;
                }
            }
            else
            {
                newExp.Add(n);

            }
        }

        return newExp;
    }
    public string createNumberExp(List<string> exp)
    {
        string express = "MATH-EXP->";
        foreach (string op in Addterms(MutTerms(exp)))
        {
            express += op;
        }
        return express;
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
            else if (fragment == "/end")
            {
                tokenizedCode += "endprogram";
            }
            else if (fragment == "+")
            {                
                tokenizedCode += "ADD_OP";
            }
            else if (fragment == "-")
            {
                tokenizedCode += "SUB_OP";
            }
            else if (fragment == "*")
            {
                tokenizedCode += "MUL_OP";
            }
            else if (fragment == "/")
            {
                tokenizedCode += "DIV_OP";
            }
            else if (fragment.Length > 0 && fragment[0].ToString() == "'" && fragment[fragment.Length - 1].ToString() == "'")
            {
                tokenizedCode += "STR:" + fragment;
            }
            else if (fragment == "true" || fragment == "false")
            {
                tokenizedCode += "BOOL:" + fragment;
            }
            else if (int.TryParse(fragment, out int n))
            {                
                tokenizedCode += "INT:" + fragment;                
            }
            else if (specialChars.Contains(fragment))
            {
                tokenizedCode += fragment;
            }
            else
            {
                string token = "VALUE->" + fragment;
                tokenizedCode += token;

            }
            tokenizedCode += ' ';
        }
        tokenizedCode = tokenizedCode.Trim();
        Debug.Log(tokenizedCode);
        return tokenizedCode;
    }
    //public bool solveBooleanExp()
    //{

    //}
    public void generateAST(string tokenList)
    {
        POSITION.Line = 1;
        POSITION.Term = 1;

        string[] tokenStack = tokenList.Split(' ');
        string AST = "";

        List<string> keyTokens = new List<string>{ "IMP", "VAR_D" };
        List<string> avaibleFunctions = new List<string> { "if:1:bool", "for_interval:2:int,int" };
        List<string> operators = new List<string> { "ADD_OP", "SUB_OP", "DIV_OP", "MUL_OP" };
        //List<string> systemTypes = new List<string> { "int", "string", "bool" };

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
                            ERROR.importError(POSITION.GetPosition(), $"biblioteca '{lib}' nao existe nos arquivos. Voce digitou o nome corretamente?");
                            break;
                        }
                    }
                }

                else if (placeHolder == "VAR_D")
                {
                    if (token.Contains("VALUE"))
                    {
                        string varName = token.Split("->")[1];
                        if (i < tokenStack.Length - 2)
                        {
                            i++;
                            token = tokenStack[i];

                            if (token == "EQ")
                            {
                                i++;
                                token = tokenStack[i];

                                List<string> expression = new List<string>();
                                if (i < tokenStack.Length - 1)
                                {
                                    if (operators.Contains(tokenStack[i + 1]))
                                    {
                                        expression.Add(token);
                                        while (i < tokenStack.Length)
                                        {
                                            i++;
                                            string tk = tokenStack[i];
                                            if (operators.Contains(tk) || (tk.Contains("INT") && !tk.Contains("VALUE")))
                                            {
                                                expression.Add(tk);
                                            }
                                            else
                                            {
                                                i--;
                                                break;
                                            }
                                        }

                                    }
                                    tokenStack[i] = createNumberExp(expression);
                                    token = tokenStack[i];
                                    Debug.Log(tokenStack[i]);
                                }
                                if (token.Contains("MATH-EXP->"))
                                {
                                    string exp = token.Split("->")[1];

                                    string[] expSteps = exp.Split(':');

                                    for (int n = 0; n < expSteps.Length; n++)
                                    {

                                    }
                                }
                                else if (!token.Contains("VALUE"))
                                {                                    
                                    if (token.Contains("INT") || token.Contains("BOOL") || token.Contains("STR"))
                                    {                                        
                                        systemVariables.Add(varName +"->"+ token);
                                        placeHolder = "";
                                    }
                                    else
                                    {
                                        ERROR.unnexpectedValue(POSITION.GetPosition(), $"o valor {token} não e valido para o contexto fornecido");
                                        break;
                                    }
                                }
                                else
                                {                                   
                                    bool foundVar = false;
                                    string varHold = "";
                                    string varCheker = token.Split("->")[1];
                                    foreach (string variable in systemVariables)
                                    {
                                        if (variable.Contains(varCheker))
                                        {
                                            foundVar = true;
                                            varHold = variable;
                                            break;
                                        }
                                    }
                                    if (foundVar)
                                    {
                                        systemVariables.Add(varName + "->" + varHold.Split("->")[1]);
                                        placeHolder = "";
                                    }
                                    else
                                    {
                                        ERROR.undefinedValue(POSITION.GetPosition(), $"o valor {varCheker} nao foi atribuido as variaveis do sistema, declare-a.");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ERROR.unnexpectedChar(POSITION.GetPosition(), "esperado o operador '=', para atribuicao de variaveis.");
                                break;
                            }

                        }
                        else
                        {
                            ERROR.syntaxError(POSITION.GetPosition(), "variavel nao atribuida.");
                            break;
                        }
                    }
                    else
                    {
                        ERROR.unnexpectedChar(POSITION.GetPosition(), "adicione um nome para a variavel, que nao seja uma palavra ou caracter reservado a linguagem!");
                        break;
                    }
                }
                

                else if (placeHolder.Contains("VALUE"))
                {
                    Debug.Log($"PlaceHolder has: {placeHolder}");
                    if (token == "L_PAR")
                    {
                        placeHolder = "FUNC:" + placeHolder.Split("->")[1];
                    }
                    else
                    {
                        ERROR.undefinedExpression(POSITION.GetPosition(), $"termo desconhecido '{placeHolder}'. Voce se esqueceu de importar alguma biblioteca?");
                        break;
                    }
                }

                else if (placeHolder.Contains("FUNC"))
                {
                    string metodoth = placeHolder.Split(':')[1];
                    bool foundMatch = false;

                    string funConfigs = "";
                    foreach (string f in avaibleFunctions)
                    {
                        if (f.Contains(metodoth))
                        {
                            foundMatch = true;
                            funConfigs = f;
                            break;
                        }
                    }

                    if (foundMatch)
                    {
                        int parAmount = int.Parse(funConfigs.Split(':')[1]);
                        int parIndex = 0;
                        
                        string[] parTypes;
                        if (parAmount > 0)
                        {
                            parTypes = funConfigs.Split(':')[2].Split(',');
                        }
                        while (true)
                        {
                            if (i < tokenStack.Length - 1)
                            {
                                i++;
                                token = tokenStack[i];
                                if (token.Contains("VALUE"))
                                {

                                }
                                else if (token == "CO")
                                {

                                }
                                else if (token == "R_PAR")
                                {

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                break;
                            }
                        }


                    }
                    else
                    {
                        ERROR.undefinedExpression(POSITION.GetPosition(), $"A funcao {metodoth} nao e definida. Voce se esqueceu de importar alguma biblioteca?");
                        break;
                    }
                }               
                
            }
            else
            {
                if (keyTokens.Contains(token) || token.Contains("VALUE"))
                {
                    placeHolder = token;                    
                }
                else if (token == "BRK")
                {
                    POSITION.Line++;
                    POSITION.Term = 1;
                }
                else if (token == "endprogram")
                {
                    break;
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
