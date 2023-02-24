using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class StartDialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ConfTxt;
    [SerializeField] private GameObject[] buttons;

    private int myId;
    void Start()
    {
        hideBox();
    }

    public void typeTxt(string txt)
    {
        ConfTxt.text = ">> ";
        ConfTxt.gameObject.SetActive(true);

        StartCoroutine(type(txt, 0.02f));
    }
    private IEnumerator type(string txt, float delay)
    {
        foreach (char letter in txt)
        {
            ConfTxt.text += letter;
            yield return new WaitForSeconds(delay);
        }
        DisplayChoices(new string[2] { ">> sim", ">> nao" });
    }

    public void DisplayChoices(string[] choicetxt)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
            TextMeshProUGUI bTxt= buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            bTxt.text = choicetxt[i];
            
        }
    }
    public void SetMyClass(int id)
    {
        myId = id;
    }
    public void MakeChoice(string klass)
    {
        if (myId == 0)
        {
            if (klass == "yes")
            {
                NewGame();
            }
            else
            {
                hideBox();
            }
        }
        else if (myId == 1)
        {
            if (klass == "yes")
            {
                Quit();
            }
            else
            {
                hideBox();
            }
        }
        
    }

    private void hideBox()
    {
        ConfTxt.gameObject.SetActive(false);

        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }
    public void StartGame()
    {
        GameSave save = SavingController.LoadGame();
        SavedSceneData.savedScene = save.SavedScene;       
        Vector2 PlayerPos = new Vector2 (save.playerPosition[0], save.playerPosition[1]);
        Vector2 VickPos = new Vector2(save.vickyPosition[0], save.vickyPosition[1]);
        LastScene.lastPlayerPos = new Vector2(PlayerPos[0], PlayerPos[1]);
        LastScene.lastVickPos = new Vector2(VickPos[0], VickPos[1]);
        ChangeScene.Instance.ChangeToScene(SavedSceneData.savedScene);
    }

    private void NewGame()
    {        
        EventMannager.DeleteSave();
        StartGame();

    }

    private void Quit()
    {
        Application.Quit();
    }

}
