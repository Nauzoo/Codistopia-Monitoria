using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NAZULI_Interpreter : MonoBehaviour
{
    public static class LENGENERALS
    {
        public static List<string> CODE_VARIABLES = new List<string>();
        public static string GetSpecialChars()
        {
            string specialChars = "{} () + - += -= / * ! <> != == ;";
            return specialChars;
        }
        public static string GetKeyWords()
        {
            string keyWords = "var if else or and";
            return keyWords;
        }
        public static string GetLegalTypes()
        {
            string legalTypes = "INT FLOAT STRING BOOL CALLER";
            return legalTypes;
        }
        public static string GetOperators()
        {
            string legalOps = "MUL_SIG DIV_SIG PLUS_SIG MIN_SIG";
            return legalOps;
        }
        public static string GetLogicalOperators()
        {
            string legalOps = "OR AND NOTEQ EQ";
            return legalOps;
        }

        public static void AddNewVariable(string name, string type, string value)
        {
            string new_variable = $"{name}:{type}:{value}";
            CODE_VARIABLES.Add(new_variable);
        }
    }

    public static class ERRORS {
        static string hasError = null;
        public static void IllegalElement(string character, string details, int line, int word)
        {
            hasError = $"IllegalElement: '{character}' in Line: {line}, Word: {word} {details}";
        }

        public static void InvalidSyntax(string character, string details, int line, int word)
        {
            hasError = $"InvalidSyntax: '{character}' in Line: {line}, Word: {word} {details}";
        }

        public static void UnexpectedElement(string character, string details, int line, int word)
        {
            hasError = $"UnexpectedElement: '{character}' in Line: {line}, Word: {word} {details}";
        }
        public static void VariableNotFound(string character, string details, int line, int word)
        {
            hasError = $"VariableNotFound: '{character}' in Line: {line}, Word: {word} {details}";
        }
        public static string GetErrors()
        {
            return hasError;
        }
        public static void Reset()
        {
            hasError = null;
        }
    }
    public class POSITION
    {
        int myLine;
        int myCol;

        public POSITION(int line, int col)
        {
            myCol = col;
            myLine = line;
        }

        public void AdvanceCol()
        {
            myCol++;
        }
        public void AdvanceLine()
        {
            myCol = 0;
            myLine++;
        }
        public int[] GetCurrentPos()
        {
            int[] pos = { myLine, myCol };
            return pos;
        }
    }
    public class TOKEN
    {
        public string identifier;
        public string tokenValue;
        public string declaratorValue;
        public int myLine;
        public int myWord;
        public float decimalPoints = 1;
        public TOKEN(string id, int line, int word, string value)
        {
            identifier = id;
            tokenValue = value;

            myLine = line;
            myWord = word;
        }

        public string readToken()
        {
            string readString = $"{identifier} {tokenValue} ";
            if (declaratorValue != null)
            {
                readString += declaratorValue;
            }
            return readString;
        }
    }
    public class TOKENIZER
    {
        List<string> codeLine;
        POSITION pos = new POSITION(1, 0);
        public TOKENIZER(List<string> line)
        {
            codeLine = line;
        }

        public List<TOKEN> GetTokens()
        {
            List<TOKEN> tokenString = new List<TOKEN>();
            for (int i = 0; i < codeLine.Count; i++)
            {
                pos.AdvanceCol();
                if (codeLine[i].Length > 0)
                {
                    if (codeLine[i].Contains("'"))
                    {
                        if (codeLine[i][0].ToString() == "'" && codeLine[i].Last().ToString() == "'" && codeLine[i].Length > 1)
                        {
                            tokenString.Add(new TOKEN("STRING", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                        }
                        else
                        {
                            ERRORS.UnexpectedElement(codeLine[i], "expected: 'string'", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1]);
                        }
                    }

                    else
                    {
                        if (int.TryParse(codeLine[i], out _))
                        {
                            tokenString.Add(new TOKEN("INT", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                        }

                        else if (float.TryParse(codeLine[i], out _))
                        {
                            int dotCounts = 0;
                            bool canGen = true;
                            foreach (char c in codeLine[i])
                            {
                                if (c == ',')
                                {
                                    ERRORS.IllegalElement(",", "did you mean '.'?", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1]);
                                    canGen = false;
                                    break;

                                }
                                else if (c == '.')
                                {
                                    dotCounts++;
                                }

                            }
                            if (dotCounts < 2 && canGen)
                            {                                
                                tokenString.Add(new TOKEN("FLOAT", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                tokenString[tokenString.Count - 1].decimalPoints = Mathf.Pow(10, codeLine[i].Split(".")[1].Length);                                
                            }
                            else
                            {
                                ERRORS.IllegalElement(codeLine[i], "are you typing more than one '.'?", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1]);
                            }
                        }

                        else if (codeLine[i] == "write:")
                        {
                            tokenString.Add(new TOKEN("WRITE", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], "write"));
                        }
                        else if (LENGENERALS.GetSpecialChars().Contains(codeLine[i]))
                        {
                            switch (codeLine[i])
                            {
                                case "+":
                                    tokenString.Add(new TOKEN("PLUS_SIG", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "-":
                                    tokenString.Add(new TOKEN("MIN_SIG", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "=":
                                    tokenString.Add(new TOKEN("EQ_SIG", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "*":
                                    tokenString.Add(new TOKEN("MUL_SIG", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "/":
                                    tokenString.Add(new TOKEN("DIV_SIG", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "(":
                                    tokenString.Add(new TOKEN("L_PAR", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case ")":
                                    tokenString.Add(new TOKEN("R_PAR", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case ";":
                                    tokenString.Add(new TOKEN("ENDLINE", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "{":
                                    tokenString.Add(new TOKEN("R_BRAC", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                                case "}":
                                    tokenString.Add(new TOKEN("L_BRAC", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;

                            }
                        }
                        else if (LENGENERALS.GetKeyWords().Contains(codeLine[i]) && codeLine[i].Length > 1)
                        {
                            switch (codeLine[i])
                            {
                                case "var":
                                    if (codeLine[i + 1].Contains("'"))
                                    {
                                        pos.AdvanceCol();
                                        ERRORS.IllegalElement(codeLine[i + 1], "can't use string as variable's name", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1]);
                                        return new List<TOKEN>();
                                    }
                                    if (int.TryParse(codeLine[i + 1], out _) || float.TryParse(codeLine[i + 1], out _))
                                    {
                                        pos.AdvanceCol();
                                        ERRORS.IllegalElement(codeLine[i + 1], "can't use numbers as variable's name", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1]);
                                        return new List<TOKEN>();
                                    }

                                    tokenString.Add(new TOKEN("KEY_WORD", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    tokenString[tokenString.Count - 1].declaratorValue = codeLine[i + 1];
                                    i++;

                                    break;
                                case "if":
                                    tokenString.Add(new TOKEN("KEY_WORD", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;

                                case "else":
                                    tokenString.Add(new TOKEN("KEY_WORD", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                                    break;
                            }
                        }
                        else if (codeLine[i] == "<break>")
                        {
                            pos.AdvanceLine();
                        }
                        else
                        {
                            tokenString.Add(new TOKEN("CALLER", pos.GetCurrentPos()[0], pos.GetCurrentPos()[1], codeLine[i]));
                        }
                    }
                }
            }
            return tokenString;
        }
    }
    public class LEXER
    {
        private string runnigCode;
        public LEXER(string codeLine) {
            runnigCode = codeLine;
        }
        public List<TOKEN> TOKENIZE()
        {
            List<string> codeParts = new List<string>();
            bool ignoreSpaces = false;
            string wordholder = "";

            foreach (char c in runnigCode)
            {
                if (c.ToString() == "'" && !ignoreSpaces)
                {
                    ignoreSpaces = true;
                }
                else if (c.ToString() == "'" && ignoreSpaces)
                {
                    wordholder += c;
                    codeParts.Add(wordholder);
                    wordholder = "";
                    ignoreSpaces = false;
                }
                if (ignoreSpaces)
                {
                    wordholder += c;
                }
                if (c != ' ' && c.ToString() != "'" && !ignoreSpaces)
                {
                    if (LENGENERALS.GetSpecialChars().Contains(c))
                    {
                        if (wordholder.Length > 0)
                        {
                            codeParts.Add(wordholder);
                            wordholder = "";
                        }
                        codeParts.Add(c.ToString());
                    }
                    else if (c == '\n')
                    {
                        codeParts.Add("<break>");
                    }
                    else {
                        wordholder += c;
                    }
                }
                else if (c == ' ' && !ignoreSpaces)
                {
                    if (wordholder.Length > 0)
                    {
                        codeParts.Add(wordholder);
                        wordholder = "";
                    }
                }
            }
            if (wordholder.Length > 0)
            {
                codeParts.Add(wordholder);
            }
            for (int i = 0; i < codeParts.Count; i++)
            {
                codeParts[i] = codeParts[i].Trim();
            }

            codeParts.RemoveAt(codeParts.Count - 1);
            
            TOKENIZER tokens = new TOKENIZER(codeParts);
            List<TOKEN> tokenList = tokens.GetTokens();

            return tokenList;
        }
    }
    public class NODE
    {
        public string indentifier;
        public NODE lAbsObject; 
        public NODE rObject; 
        public TOKEN absValue;
        public List<NODE> myCodeBlock;
        public NODE(string name)
        {
            indentifier = name;            
        }
    }   
    public class NumberNode
    {
        public string Type;        
        public float FloatValue;
        public NumberNode(string NumberType, float myNumberF)
        {
            Type = NumberType;            
            FloatValue = myNumberF;
        }
    }

    public class PARSER
    {
        private List<TOKEN> myList;
        private TOKEN currentToken;
        int tokenIndex = -1;
        public List<NODE> AST = new List<NODE>();
        private bool finishedtoParase = false;
        public PARSER(List<TOKEN> codeList)
        {
            myList = codeList;
        }
        public void StartParsing()
        {
            Advance();
            if (!finishedtoParase && ERRORS.GetErrors() == null)
            {
                if (currentToken.identifier == "KEY_WORD")
                {
                    switch (currentToken.tokenValue)
                    {
                        case "var":
                            VarDecParsing();
                            break;
                        case "if":
                            CondStatement();
                            break;
                    }
                }
                else
                {
                    ERRORS.InvalidSyntax(currentToken.tokenValue, "Unknown key word", currentToken.myLine, currentToken.myWord);
                }
            }            
        }
        private void Advance()
        {
            tokenIndex++;
            if (tokenIndex < myList.Count)
            {
                currentToken = myList[tokenIndex];
            }
            else
            {
                finishedtoParase = true; 
            }
            
        }
        private NODE MakeNode(string type, TOKEN myToken, NODE lNode, NODE rNode)
        {
            NODE myNewNode = new NODE(type);
            if (myToken != null)
            {
                myNewNode.absValue = myToken;
            }
            else if (lNode != null && !type.Contains("BinaryExp"))
            {
                myNewNode.lAbsObject = lNode;
            }
            else
            {
                myNewNode.lAbsObject = lNode;
                myNewNode.rObject = rNode;
            }
            return myNewNode;

        }
        
        private void CondStatement()
        {
            AST.Add(MakeNode($"CondIf", null, null, null));
            Advance();
            if (currentToken.identifier == "CALLER" || currentToken.identifier == "BOOL")
            {
                List<TOKEN> varExp = new List<TOKEN>();
                while (true)
                {
                    if ("CALLER BOOL".Contains(currentToken.identifier) || LENGENERALS.GetLogicalOperators().Contains(currentToken.identifier))
                    {
                        varExp.Add(currentToken);
                        Advance();
                    }
                    else if (currentToken.identifier == "L_BRAC")
                    {
                        break;
                    }
                    else
                    {
                        ERRORS.UnexpectedElement(currentToken.tokenValue, "is illegal to this context.", currentToken.myLine, currentToken.myWord);
                        break;
                    }
                    if (tokenIndex >= myList.Count())
                    {
                        ERRORS.InvalidSyntax(currentToken.tokenValue, "expected '{'.", currentToken.myLine, currentToken.myWord);
                        break;
                    }
                }
            }
            else
            {
                ERRORS.InvalidSyntax(currentToken.tokenValue, "expected boolean type", currentToken.myLine, currentToken.myWord);
            }
        }


        private void VarDecParsing()
        {
            AST.Add(MakeNode($"VariableDeclaring:{currentToken.declaratorValue}", null, null, null));
            Advance();            
            if (currentToken.identifier == "EQ_SIG")
            {
                Advance();

                List<TOKEN> varExp = new List<TOKEN>();
                while (true)
                {
                    if (LENGENERALS.GetLegalTypes().Contains(currentToken.identifier) || LENGENERALS.GetOperators().Contains(currentToken.identifier))
                    {
                        varExp.Add(currentToken);
                        Advance();
                    }
                    else if (currentToken.identifier == "ENDLINE")
                    {
                        break;
                    }
                    else
                    {
                        ERRORS.UnexpectedElement(currentToken.tokenValue, "is illegal to this context.", currentToken.myLine, currentToken.myWord);
                        break;
                    }
                    if (tokenIndex >= myList.Count())
                    {
                        ERRORS.InvalidSyntax(currentToken.tokenValue, "expected ';'.", currentToken.myLine, currentToken.myWord);
                        break;
                    }
                }
                if (ERRORS.GetErrors() == null)
                {
                    if (varExp.Count > 0)
                    {
                        if (varExp[0].identifier == "INT" || varExp[0].identifier == "FLOAT")
                        {
                            AST[AST.Count - 1].lAbsObject = MakeNode("NumberExpression", null, ExpParsing(varExp), null);
                        }
                        else if (varExp[0].identifier == "CALLER")
                        {
                            if (varExp.Count > 1)
                            {
                                AST[AST.Count - 1].lAbsObject = MakeNode("NumberExpression", null, ExpParsing(varExp), null);
                            }
                            else
                            {
                                AST[AST.Count - 1].lAbsObject = MakeNode("SimpleObj", varExp[0], null, null);
                            }
                        }
                        else if (varExp[0].identifier == "STRING" || varExp[0].identifier == "BOOL")
                        {
                            AST[AST.Count - 1].lAbsObject = MakeNode("SimpleObj", varExp[0], null, null);
                        }
                        else
                        {
                            ERRORS.UnexpectedElement(currentToken.tokenValue, "is illegal to this context. should you try INT or FLOAT types?", currentToken.myLine, currentToken.myWord);
                        }
                    }
                    else
                    {
                        ERRORS.InvalidSyntax(currentToken.tokenValue, "not inicialiezed variable.", currentToken.myLine, currentToken.myWord);
                    }
                }
            }
            else
            {
                ERRORS.InvalidSyntax(currentToken.tokenValue, "expected '='.", currentToken.myLine, currentToken.myWord);
            }
            StartParsing();
                        
        }

        //private NODE LogicalOp (List<TOKEN> myExpression) { }
        private NODE BinaryExp(NODE factor1, TOKEN op, NODE factor2)
        {
            if (op.identifier == "MUL_SIG")
            {
                return MakeNode("BinaryExp:MUL", null, factor1, factor2);
            }
            else if (op.identifier == "DIV_SIG")
            {
                return MakeNode("BinaryExp:DIV", null, factor1, factor2);
            }
            else if (op.identifier == "MIN_SIG")
            {
                return MakeNode("BinaryExp:MIN", null, factor1, factor2);
            }
            else
            {
                return MakeNode("BinaryExp:PLUS", null, factor1, factor2);
            }

        }
        private NODE ExpParsing(List<TOKEN> myExpression)
        {
            List<NODE> expression = new List<NODE>();
            foreach (TOKEN toke in myExpression)
            {
                if (LENGENERALS.GetLegalTypes().Contains(toke.identifier))
                {
                    expression.Add(MakeNode("factorNode", toke, null, null));
                }
                else if (LENGENERALS.GetOperators().Contains(toke.identifier))
                {
                    expression.Add(MakeNode("Operator", toke, null, null));
                }
            }
            List<NODE> firstIt = new List<NODE>();
            NODE holder = new NODE("generic");
            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i].indentifier == "factorNode")
                {
                    holder = expression[i];              
                    if (i < expression.Count - 2)
                    {
                        i++;
                        if (expression[i].indentifier == "Operator" && expression[i].absValue.identifier == "MUL_SIG" || expression[i].absValue.identifier == "DIV_SIG")
                        {
                            if (holder.indentifier == "generic")
                            {
                                holder = expression[i-1];
                            }
                            i++;
                            if (expression[i].indentifier == "factorNode")
                            {                                
                                NODE exp = BinaryExp(holder, expression[i - 1].absValue, expression[i]);
                                firstIt.Add(exp);
                                holder = exp;
                            }
                        }
                        else
                        {
                            firstIt.Add(expression[i-1]);
                            firstIt.Add(expression[i]);
                            holder = expression[i];
                        }
                    }
                    else
                    {
                        firstIt.Add(holder);
                    }
                }
                else if (expression[i].absValue.identifier == "MUL_SIG" || expression[i].absValue.identifier == "DIV_SIG" && holder.indentifier != "generic")
                {
                    if (i < expression.Count - 1)
                    {
                        i++;
                        if (expression[i].indentifier == "factorNode")
                        {
                            if (holder.indentifier.Contains("BinaryExp"))
                            {
                                firstIt.RemoveAt(firstIt.Count - 1);
                            }
                            NODE exp = BinaryExp(holder, expression[i - 1].absValue, expression[i]);
                            firstIt.Add(exp);
                            holder = exp;
                        }
                    }
                }
                else
                {
                    holder = expression[i];
                    firstIt.Add(holder);
                }                
            }
            if (holder != firstIt[firstIt.Count - 1])
            {
                firstIt.Add(holder);
            }
                    
            List<NODE> FinalParse = new List<NODE>();
            holder = new NODE("generic");
            for (int i = 0; i < firstIt.Count; i++)
            {
                if (firstIt[i].indentifier == "factorNode" || firstIt[i].indentifier.Contains("BinaryExp"))
                {
                    holder = firstIt[i];
                    if (i < firstIt.Count - 2 && firstIt.Count > 1)
                    {                        
                        i++;
                        if (firstIt[i].indentifier == "Operator")
                        {
                            if (holder.indentifier == "generic")
                            {
                                holder = firstIt[i-1];
                            }
                            i++;
                            if (firstIt[i].indentifier.Contains("BinaryExp"))
                            {
                                NODE exp = BinaryExp(holder, firstIt[i - 1].absValue, firstIt[i]);
                                FinalParse.Add(exp);
                                holder = exp;
                            }
                            else if (firstIt[i].indentifier == "factorNode")
                            {
                                NODE exp = BinaryExp(holder, firstIt[i - 1].absValue, firstIt[i]);
                                FinalParse.Add(exp);
                                holder = exp;
                            }
                        }
                        
                    }
                    else
                    {
                        FinalParse.Add(holder);                        
                    }
                }

                else if (firstIt[i].indentifier == "Operator" && holder.indentifier != "generic")
                {
                    if (i < firstIt.Count - 1 && firstIt.Count > 1)
                    {
                        i++;
                        if (firstIt[i].indentifier == "factorNode")
                        {
                            NODE exp = BinaryExp(holder, firstIt[i - 1].absValue, firstIt[i]);
                            FinalParse.Add(exp);
                            holder = exp;
                        }
                    }
                }                
            }
            NODE MyParseRes = new NODE("generic");
            MyParseRes = FinalParse[FinalParse.Count - 1];            
            return MyParseRes;

        }
    }

    public class INTERPRETER
    {
        List<NODE> commands;
        public INTERPRETER(List<NODE> AST)
        {
            commands = AST;
        }

        private NumberNode SoveFactor(NODE myOperation)
        {
            NumberNode new_factor = new NumberNode("generic", 1);
            
            if (myOperation.indentifier.Contains("BinaryExp"))
            {
                if (myOperation.indentifier.Contains("PLUS"))
                {
                    new_factor = AddNumbers(myOperation);
                }
                else if (myOperation.indentifier.Contains("MIN"))
                {
                    new_factor = ReduceNumbers(myOperation);
                }
                else if (myOperation.indentifier.Contains("MUL"))
                {
                    new_factor = MultiplyNumbers(myOperation);
                }
                else
                {
                    new_factor = DivideNumbers(myOperation);
                }
            }
            else
            {
                if (myOperation.absValue.identifier != "CALLER")
                {
                    float newValue = float.Parse(myOperation.absValue.tokenValue);
                    newValue = newValue / myOperation.absValue.decimalPoints;
                    new_factor = new NumberNode(myOperation.absValue.identifier, newValue);
                }
                else
                {
                    myOperation = getVariable(myOperation.absValue);
                    if (myOperation.indentifier != "generic")
                    {
                        float newValue = float.Parse(myOperation.absValue.tokenValue);
                        newValue = newValue / myOperation.absValue.decimalPoints;
                        new_factor = new NumberNode(myOperation.absValue.identifier, newValue);
                    }
                    else
                    {
                        return new NumberNode("generic", 1);
                    }

                }
            }
            return new_factor;
        }
        private NumberNode AddNumbers(NODE BinaryOp)
        {
            NODE leftFactor = BinaryOp.lAbsObject;
            NODE rightFactor = BinaryOp.rObject;

            NumberNode new_lFactor = SoveFactor(leftFactor);
            NumberNode new_rFactor = SoveFactor(rightFactor);
            NumberNode result = new NumberNode("generic", 0);
            
            float express = new_lFactor.FloatValue + new_rFactor.FloatValue;
            if (int.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("INT", express);
            }
            else if (float.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("FLOAT", express);
            }
            return result;
        }
        private NumberNode ReduceNumbers(NODE BinaryOp)
        {
            NODE leftFactor = BinaryOp.lAbsObject;
            NODE rightFactor = BinaryOp.rObject;

            NumberNode new_lFactor = SoveFactor(leftFactor);
            NumberNode new_rFactor = SoveFactor(rightFactor);
            NumberNode result = new NumberNode("generic", 0);

            float express = new_lFactor.FloatValue - new_rFactor.FloatValue;
            if (int.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("INT", express);
            }
            else if (float.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("FLOAT", express);
            }
            return result;
        }
        private NumberNode MultiplyNumbers(NODE BinaryOp)
        {
            NODE leftFactor = BinaryOp.lAbsObject;
            NODE rightFactor = BinaryOp.rObject;

            NumberNode new_lFactor = SoveFactor(leftFactor);
            NumberNode new_rFactor = SoveFactor(rightFactor);
            NumberNode result = new NumberNode("generic", 0);

            float express = new_lFactor.FloatValue * new_rFactor.FloatValue;
            if (int.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("INT", express);
            }
            else if (float.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("FLOAT", express);
            }
            return result;
        }
        private NumberNode DivideNumbers(NODE BinaryOp)
        {
            NODE leftFactor = BinaryOp.lAbsObject;
            NODE rightFactor = BinaryOp.rObject;

            NumberNode new_lFactor = SoveFactor(leftFactor);
            NumberNode new_rFactor = SoveFactor(rightFactor);
            NumberNode result = new NumberNode("generic", 0);

            float express = new_lFactor.FloatValue / new_rFactor.FloatValue;
            if (int.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("INT", express);
            }
            else if (float.TryParse(express.ToString(), out _))
            {
                result = new NumberNode("FLOAT", express);
            }
            return result;
        }

        private NODE getVariable(TOKEN callerToke)
        {
            NODE myNode = new NODE("generic");
            if (checkForVar(callerToke.tokenValue))
            {
                foreach (string var in LENGENERALS.CODE_VARIABLES)
                {
                    if (var.Contains(callerToke.tokenValue))
                    {
                        string indentifier = var.Split(":")[1];
                        string tokenValue = var.Split(":")[2];
                        TOKEN myToke = new TOKEN(indentifier, 0, 0, tokenValue);

                        myNode.indentifier = "valid";
                        myNode.absValue = myToke;
                        break;
                    }
                }
            }
            else
            {
                ERRORS.VariableNotFound(callerToke.tokenValue, "did you miss declaring this element?", callerToke.myLine, callerToke.myWord);
            }
            return myNode;
        }
        private bool checkForVar(string varName)
        {
            bool founVar = false;
            foreach (string var in LENGENERALS.CODE_VARIABLES)
            {
                if (var.Contains(varName))
                {
                    founVar = true;
                    break;
                }
            }
            return founVar;
        }

        public void EXCECUTE()
        {
            foreach (NODE command in commands)
            {
                if (ERRORS.GetErrors() == null)
                {
                    if (command.indentifier.Contains("VariableDeclaring"))
                    {
                        string varName = command.indentifier.Split(":")[1];
                        NODE nxtStep = command.lAbsObject;
                        NumberNode res;
                        if (!checkForVar(varName))
                        {
                            if (nxtStep.indentifier == "NumberExpression")
                            {
                                nxtStep = nxtStep.lAbsObject;
                                if (nxtStep.indentifier == "BinaryExp:PLUS")
                                {
                                    res = AddNumbers(nxtStep);
                                }
                                else if (nxtStep.indentifier == "BinaryExp:MIN")
                                {
                                    res = ReduceNumbers(nxtStep);
                                }
                                else if (nxtStep.indentifier == "BinaryExp:MUL")
                                {
                                    res = MultiplyNumbers(nxtStep);
                                }
                                else if (nxtStep.indentifier == "BinaryExp:DIV")
                                {
                                    res = DivideNumbers(nxtStep);
                                }
                                else
                                {
                                    res = new NumberNode(nxtStep.absValue.identifier, float.Parse(nxtStep.absValue.tokenValue)/nxtStep.absValue.decimalPoints);
                                }
                                LENGENERALS.AddNewVariable(varName, res.Type, res.FloatValue.ToString());
                            }
                            else if (nxtStep.indentifier == "SimpleObj")
                            {
                                LENGENERALS.AddNewVariable(varName, nxtStep.absValue.identifier, nxtStep.absValue.tokenValue);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log(ERRORS.GetErrors());
                    break;
                }
            }
        }    
    }
    

    public void START_RUNNING(string PROGRAM)
    {
        ERRORS.Reset();
        LEXER myLexer = new LEXER(PROGRAM);
        List<TOKEN> tokenList = myLexer.TOKENIZE();
        PARSER myParser = new PARSER(tokenList);        

        //Debug.Log(ERRORS.GetErrors());
        if(ERRORS.GetErrors() == null)
        {
            myParser.StartParsing();
            
            if (ERRORS.GetErrors() == null)
            {
                INTERPRETER myInterpreter = new INTERPRETER(myParser.AST);
                myInterpreter.EXCECUTE();
                foreach (string variable in LENGENERALS.CODE_VARIABLES)
                {
                    Debug.Log(variable);
                }                
            }
        }
        Debug.Log(ERRORS.GetErrors());              
    }
}
 