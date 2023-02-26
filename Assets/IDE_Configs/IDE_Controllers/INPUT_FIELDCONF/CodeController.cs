using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CodeController : MonoBehaviour
{
    [SerializeField] private GameObject mainInput;
    [SerializeField] private TextMeshProUGUI codeLines;
    [SerializeField] private TextMeshProUGUI lineCounter;
    [SerializeField] private RectTransform codeArea;
    [SerializeField] private GameObject v_scrollbar;
    [SerializeField] private GameObject h_scrollbar;
    [SerializeField] private TextMeshProUGUI codeBackup;
    [SerializeField] private TextMeshProUGUI CodeCounter;
    private RectTransform barTransform;
    private bool isEditing = false;
    private float VeditingPlace;
    private float HeditingPlace;
    private bool keySwitcher = false;
    private Vector2 originalCodeSize, originalBarSize;
    private int currentLine;
    private int LineCount;
    private int CaretPosition;
    public float indexateTime = 2f;    

    private void BackupCode(string newCode)
    {
        if (isEditing && newCode.Length > 1 && !newCode.Contains("</color>"))
        {
            codeBackup.text = newCode;
            Debug.Log("BackUp;");
            //CodeSet();
        }

    }
    private void CodeSet()
    {
        if (codeBackup.text != null)
        {
            mainInput.gameObject.GetComponent<TMP_InputField>().text = codeBackup.text;
        }

    }
    private void indeXate(string codeLines)
    {
        string[] codeTxt = codeBackup.text.Split(" ");

        string formatedCode = "";

        foreach (string term in codeTxt)
        {

            if (term.Contains("if"))
            {
                formatedCode += $" <color=blue>{term}</color> ";
            }
            else if (term.Contains("var"))
            {
                formatedCode += $" <color=red> {term} </color> ";
            }
            else if (term.Contains("<color=") || term.Contains("</color>")) {
                continue;
            }
            else
            {
                formatedCode += term;
            }
        }

        mainInput.gameObject.GetComponent<TMP_InputField>().text = formatedCode;

    }
    void Start()
    {
        TMP_InputField inputField = mainInput.gameObject.GetComponent<TMP_InputField>();
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
        barTransform = v_scrollbar.GetComponent<RectTransform>();

        inputField.onValueChanged.AddListener(BackupCode);
        inputField.onEndEdit.AddListener(indeXate);
        //inputField.onValueChanged.AddListener(CodeSet);

        //inputField.onEndEdit.AddListener(CodeSet);

        codeBackup.text = mainInput.gameObject.GetComponent<TMP_InputField>().text;

        originalCodeSize = new Vector2(codeArea.rect.width - codeArea.rect.width, codeArea.rect.height);
        originalBarSize = new Vector2(barTransform.rect.width, barTransform.rect.height);


        UpdateLines();
    }
    private void Update()
    {
        UpdateLines();

        if (isEditing) {
            if (LineCount != 0 && currentLine != 1)
            {
                VeditingPlace = 1 - (float)currentLine / LineCount;

            }

            else
            {
                VeditingPlace = 1;
            }

            v_scrollbar.GetComponent<Scrollbar>().value = VeditingPlace;
            h_scrollbar.GetComponent<Scrollbar>().value = HeditingPlace;
        
        }

        if (TouchScreenKeyboard.visible && !keySwitcher)
        {
            keySwitcher = true;
            EnterEditMode();
        }

        else if (!TouchScreenKeyboard.visible && keySwitcher) {
            keySwitcher = false;
            ExitEditMode();
        }
        
    }

    public void EnterEditMode() {
        codeArea.sizeDelta = new Vector2(codeArea.rect.width - codeArea.rect.width, codeArea.rect.height / 2.6f);
        barTransform.sizeDelta = new Vector2(barTransform.rect.width, barTransform.rect.height / 2.6f);
        CodeSet();
        isEditing = true;

        VeditingPlace = v_scrollbar.GetComponent<Scrollbar>().value;
        HeditingPlace = h_scrollbar.GetComponent<Scrollbar>().value;

    }

    public void ExitEditMode() {
        codeArea.sizeDelta = originalCodeSize;
        barTransform.sizeDelta = originalBarSize;
        indeXate(codeBackup.text);

        isEditing = false;
    }

    public void UpdateLines()
    {
        LineCount = codeLines.textInfo.lineCount;
        CaretPosition = mainInput.gameObject.GetComponent<TMP_InputField>().caretPosition;

        List<int> breakLines = new List<int>();

        for (int i = 0; i < codeLines.text.Length; i++)
        {
            if (codeLines.text[i].ToString() == "\n")
            {
                breakLines.Add(i);
            }
        }

        if (breakLines.Count != 0)
        {
            if (CaretPosition < breakLines[0])
            {
                currentLine = 1;
            }

            else if (CaretPosition > breakLines.Last())
            {
                currentLine = LineCount;
            }

            else
            {
                for (int i = 0; i < breakLines.Count; i++)
                {
                    if (CaretPosition > breakLines[i] && CaretPosition <= breakLines[i + 1])
                    {
                        currentLine = breakLines.IndexOf(breakLines[i + 1]) + 1;

                    }

                }

            }
        }

        //Debug.Log($"Currently in Line: {currentLine}, Caret position: {CaretPosition}");
        lineCounter.text = "1";
        string newCount = "1";
        for (int i = 2; i <= LineCount; i++) {
            newCount += $"\n{i}";
        }

        lineCounter.text = newCount;

        CodeCounter.text = LineCount.ToString();

        //codeBackup.text = codeLines.text;
    }
    
}
