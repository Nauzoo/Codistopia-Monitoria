using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneEventsM : EventMannager
{
    [SerializeField] private SquareColor squareColor;
    [SerializeField] private ChangeScene sceneChanger;
    [SerializeField] private AudioClip PcOn;
    [SerializeField] private AudioClip saveFx;
    [SerializeField] private IDEeventsMan IdeEvent;
    [SerializeField] private NPC_Movement Marisa;
    [SerializeField] private Image vickyFaceIde;
    [SerializeField] private Sprite vickTutorial;
    [SerializeField] private DoorSwitcher[] Wdoor;

    [SerializeField] private EventMannager evMan;

    public override void triggerEvent(string name) {
        switch (name.Trim())
        {
            case "OpenPc":
                savingSceneConf();
                SoundMannager.Instance.PlaySound(PcOn);
                sceneChanger.ChangeToScene(1);
                break;

            case "SquareCollor":
                squareColor.ChangeColor();
                EventsData.happenedEvents.Add(name);
                break;

            case "SaveGame":
                SavingController.SavePlayer(Player_movement.GetInstance());
                SavingController.SaveVick(VickFollower.GetInstance());
                SavingController.SaveEvents();
                int sceneId = SceneManager.GetActiveScene().buildIndex;
                SavingController.SaveLastScene(sceneId);
                SoundMannager.Instance.PlaySound(saveFx);
                break;
            case "MarisaT":
                EventsData.happenedEvents.Add("marisaT");
                break;

            case "bookCheck":
                Marisa.animator.SetInteger("animIndex", 0);
                Marisa.canMove = false;
                break;

            case "realeaseBook":
                Marisa.canMove = true;
                break;

            case "IdeStarTut":
                //IdeEvent.TriggerEvent("TutStart");
                vickyFaceIde.sprite = vickTutorial;
                break;
            case "IdeCloseTut":
                //IdeEvent.TriggerEvent("TutClose");
                break;

            case "unlockW":
                EventsData.happenedEvents.Add("unlockW");
                foreach (DoorSwitcher element in Wdoor)
                {
                    element.SwitchState();
                }
                break;
        }
    }

}
