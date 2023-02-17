using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDEeventsMan : EventMannager
{
    [SerializeField] private GameObject interButton;

    [SerializeField] private TextMeshProUGUI result;

    [SerializeField] private AudioClip sucsses, error;

    [SerializeField] private Sprite[] vickyFaces = new Sprite[2];
    [SerializeField] private Image vickyFace;

    private int errorCount = 0;

    public static IDEeventsMan Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There are more then one instance of the IDE_EVENTS_M class in this scene!");
        }
    }
    void Start()
    {
        StartTutorial();
        result.gameObject.SetActive(false);
    }
    public void sumError()
    {
        errorCount++;
    }
    public override void triggerEvent(string name)
    {
        switch (name)
        {
            case "TutStart":
                StartTutorial();
                break;

            case "TutClose":
                EndTutorial();
                break;

            case "ShowTip":
                ShowTip();
                break;
        }
    }

    private void StartTutorial()
    {
        interButton.SetActive(true);
        List<string> myTutLines = new List<string> (CurrentTutorial.text.Split("\n"));
        DialogMannagerV2.GetInstance().TriggerDialog(myTutLines);
    }
    private void EndTutorial()
    {
        interButton.SetActive(false);
    }


    public void showCompResult(bool hasErrors)
    {
        if (!hasErrors)
        {
            StartCoroutine(ShowResult("<color=green>Codigo compilado com suceesso!"));
            SoundMannager.Instance.PlaySound(sucsses);
        }
        else
        {
            StartCoroutine(ShowResult("<color=red>Erro de compilacao!"));
            SoundMannager.Instance.PlaySound(error);
            sumError();
            StartCoroutine(ErrorMsg());
        }
    }

    private IEnumerator ErrorMsg()
    {
        if (errorCount < 10)
        {
            vickyFace.sprite = vickyFaces[0];
        }
        else
        {
            vickyFace.sprite = vickyFaces[1];
        }
        yield return new WaitForSeconds(1);
        interButton.SetActive(true);
        List<string> tipDialog = new List<string>();
        tipDialog.Add(TipsForCode.cliche);
        tipDialog.Add("Voc� pode receber dicas clicando no bot�o '?'");       
        tipDialog.Add("-CALL:TutClose");
        tipDialog.Add("-END");
        DialogMannagerV2.GetInstance().TriggerDialog(tipDialog);
       
    }

    private void ShowTip()
    {
        interButton.SetActive(true);
        List<string> myTips = new List<string>(TipsForCode.tips.Split("\n"));
        string tip = myTips[Random.Range(0, myTips.Count)];
        List<string> tipDialog = new List<string>();
        tipDialog.Add(tip);
        tipDialog.Add("-CALL:TutClose");
        tipDialog.Add("-END");
        DialogMannagerV2.GetInstance().TriggerDialog(tipDialog);
    }

    private IEnumerator ShowResult(string res)
    {
        result.text = res;
        result.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        result.gameObject.SetActive(false);
    }
}
