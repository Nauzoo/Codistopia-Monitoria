using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDEeventsMan : MonoBehaviour
{
    [SerializeField] private GameObject interButton;
    void Start()
    {
        StartTutorial();
    }
    public void TriggerEvent(string name)
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
    private void ShowTip()
    {
        interButton.SetActive(true);
        List<string> myTips = new List<string>(TipsForCode.tips.Split("\n"));
        string tip = myTips[Random.Range(0, myTips.Count)];
        List<string> genericList = new List<string>();
        genericList.Add(TipsForCode.cliche);
        genericList.Add(tip);
        genericList.Add("-CALL:IdeCloseTut");
        genericList.Add("-END");
        DialogMannagerV2.GetInstance().TriggerDialog(genericList);
    }
}
