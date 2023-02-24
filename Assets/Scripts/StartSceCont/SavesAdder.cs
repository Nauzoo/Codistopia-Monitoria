using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class SavesAdder : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI buttonText;
    void Start()
    {
        GameSave data = SavingController.LoadGame();
        
        if (!data.hasSave)
        {
            startButton.interactable = false;
            buttonText.text = "<color=#444444>>>Carregar</color>";
        }   
    }
}
