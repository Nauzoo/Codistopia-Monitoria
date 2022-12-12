using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class COMPARER : MonoBehaviour
{
    private string formatedTempCode;
    [SerializeField] private TextMeshProUGUI result;

    [SerializeField] private AudioClip sucsses, error;
    [SerializeField] private IDEeventsMan IdeEvent;

    [SerializeField] private Sprite[] vickyFaces;
    [SerializeField] private Image vickyFace;

    private int errosCount;
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
        if (CurrentCode.text != null)
        {
            formatedTempCode = formatCode(CurrentCode.text);
        }
        else { 
            formatedTempCode = "pkasd(0hasdb**hasbdbasbb";
            Debug.LogWarning("Inicialized without Temp. Code!");
        }
        result.gameObject.SetActive(false);
        errosCount = 0;
        
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

        int typeIncreaser = 0;
        int tempIncreaser = 0;
        bool hasErros = false;
        for (int i = 0; i < formatedTempCode.Length; i++)
        {
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
                break;
            }
        }
        if (!hasErros)
        {            
            StartCoroutine(ShowResult("<color=green>Codigo compilado com suceesso!"));
            SoundMannager.Instance.PlaySound(sucsses);
        }
        else
        {
            StartCoroutine(ShowResult("<color=red>Erro de compilacao!"));
            SoundMannager.Instance.PlaySound(error);
            errosCount ++;
            StartCoroutine(ShowTip());
        }
        
    }

    private IEnumerator ShowTip()
    {
        yield return new WaitForSeconds(1);
        IdeEvent.TriggerEvent("ShowTip");
        if (errosCount < 10)
        {
            vickyFace.sprite = vickyFaces[0];
        }
        else
        {
            vickyFace.sprite = vickyFaces[1];
        }

    }
    private IEnumerator ShowResult(string res)
    {
        result.text = res;
        result.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        result.gameObject.SetActive(false);
    }
}
