using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Buttons : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeLines;
    [SerializeField] private TextMeshProUGUI visualCode;
    [SerializeField] private TextMeshProUGUI lineCounter;
    [SerializeField] private ChangeScene sceneChanger;

    [SerializeField] private AudioClip pcOff;
    private int zoomMax = 30;
    private int zoomMin = -10;
    private int currentZoom = 0;

    public void ZoomIn(){
        if (currentZoom < zoomMax)
        {
            visualCode.fontSize += 5;
            lineCounter.fontSize += 5;
            currentZoom += 5;
        }
    }

    public void ZoomOut() {
        if (currentZoom > zoomMin)
        {
            visualCode.fontSize -= 5;
            lineCounter.fontSize -= 5;
            currentZoom -= 5;
        }
    }

    public void askHELP()
    {
        IDEeventsMan.Instance.triggerEvent("ShowTip");        
    }

    public void CloseIDE()
    {
        SoundMannager.Instance.PlaySound(pcOff);
        sceneChanger.ChangeToScene(LastScene.lastPassedScene);

    }
    public void RUN_PROGRAM()
    {
        COMPARER.Instance.RUN_COMPARASSION(codeLines.text);
    }
}
