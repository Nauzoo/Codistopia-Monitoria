using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogMannagerV2 : MonoBehaviour
{
    [Header("Dialog box UI")]

    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private GameObject contSig;

    [SerializeField] private float typingSpeed = 0.2f;

    [Header("Choices UI")]

    private int choicesAmount;
    [SerializeField] private GameObject[] choices; // array that has all the SCENE options buttons
    [SerializeField] private TextMeshProUGUI[] choicesTxt; // Array that keeps the options texts

    [Header("Controllers")]
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject interButton;

    private bool isTyping;
    private string currentScentence;
    private int scentenceIndex = -1;

    public bool playingDialog { get; private set; } // Can be accessed through get method by other scripts
    public List<string> thisStory { get; set; }
    public List<string> thisChoices { get; set; }

    private List<int> linesForCall;

    private static DialogMannagerV2 instance;

    [Header("Scene Events Mannager")]
    [SerializeField] private SceneEventsM eventsTrigger;

    [Header("AudioClips")]
    [SerializeField] private AudioClip[] clip;
    private int AudioIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are more than one instace of Dialog managers on this scene!");
        }
        instance = this;
    }

    public static DialogMannagerV2 GetInstance()
    { // Used by other scripts to accesses information from this singleton class
        return instance;
    }

    private void Start()
    {
        dialogBox.SetActive(false);
        playingDialog = false;
        isTyping = false;
        choicesAmount = choices.Length;

        choicesTxt = new TextMeshProUGUI[choices.Length]; // starting the choice text array with the same lenght as the number of options avaible in the scene
        thisChoices = new List<string>();

        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesTxt[index] = choice.GetComponentInChildren<TextMeshProUGUI>(); // getting all the text spaces from all options in the scene
            index++;
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (!playingDialog)
        {
            return;
        }

        else
        {
            if (InteractionMannager.GetInstance().Interaction())
            { // checks for the interaction button input
                InteractionMannager.GetInstance().CloseInteraction(); // closes the interaction event
                if (!isTyping)
                {
                    StartCoroutine(DialogUpdate());  // go to next dialog line, if possible
                }

                else if (isTyping)
                {
                    isTyping = false;
                    dialogText.text = currentScentence;
                }
            }
        }
    }

    public void TriggerDialog(List<string> story)
    { // starts the dialog
        thisStory = story;
        
        dialogBox.SetActive(true);
        playingDialog = true;

        StartCoroutine(DialogUpdate());
        DisplayChoices(thisChoices);

        if (joystick != null)
        {
            joystick.SetActive(false);
            FixedJoystick joyScript = joystick.GetComponent<FixedJoystick>();
            joyScript.handle.anchoredPosition = Vector2.zero;
            joyScript.input = Vector2.zero;
        }        
    }

    public IEnumerator DialogUpdate()
    { // updates to the next dialog line or closes the intaration if it's over
        scentenceIndex += 1;
        thisChoices = new List<string>();
        contSig.SetActive(false);


        if (scentenceIndex < thisStory.Count)
        {
            currentScentence = thisStory[scentenceIndex];
            isTyping = true;

            if (currentScentence.Contains("-J"))
            {
                currentScentence = currentScentence.Replace("-J", "");
                dialogText.text = currentScentence;
                scentenceIndex++;
                interButton.SetActive(false);
                DisplayChoices(new List<string>());
                if (thisStory[scentenceIndex].Contains("-wait"))
                {
                    int timer = int.Parse(thisStory[scentenceIndex].Split(":")[1]);
                    yield return new WaitForSeconds(timer);
                    scentenceIndex++;
                    currentScentence = thisStory[scentenceIndex];
                }
                interButton.SetActive(true);
            }

            if (currentScentence.Contains("-goto ->")) {
                int newIndex = int.Parse(currentScentence.Split("->")[1])-1;
                scentenceIndex = newIndex;
                currentScentence = thisStory[scentenceIndex];

            }
            else if (currentScentence.Contains("-CALL:")){
                eventsTrigger.triggerEvent(currentScentence.Split(":")[1]);
                scentenceIndex++;
                currentScentence = thisStory[scentenceIndex];
            }

            if (currentScentence.Trim() == "-END"){
                CloseDialog();

            }

            if (currentScentence.Contains("->Q:"))
            {
                linesForCall = new List<int>();
                currentScentence = currentScentence.Replace("->Q:", "");

                for (int i = 1; i <= choicesAmount; i++)
                {
                    thisChoices.Add(thisStory[scentenceIndex + i].Split("-ANS:")[0]);
                    linesForCall.Add(int.Parse(thisStory[scentenceIndex + i].Split("-ANS:")[1]) - 2);
                }
                scentenceIndex += choicesAmount;
            }

            if (currentScentence.Contains("->VOICE:"))
            {               
                if (currentScentence.Split(":")[1].Trim() == "MACHINE")
                {
                    AudioIndex = 0;
                }
                else if (currentScentence.Split(":")[1].Trim() == "FEMALE")
                {
                    AudioIndex = 1;
                }
                else if (currentScentence.Split(":")[1].Trim() == "MALE")
                {
                    AudioIndex = 2;
                }
                Debug.Log(AudioIndex);
                scentenceIndex++;
                currentScentence = thisStory[scentenceIndex];

            }

            DisplayChoices(thisChoices);
                        
            dialogText.text = "";
            int lettersPassed = 0;
            foreach (char letter in currentScentence)
            {
                if (isTyping)
                {
                    dialogText.text += letter;
                    if (lettersPassed >= 5)
                    {
                        PlaySpeakFx();
                        lettersPassed = 0;
                    }
                    yield return new WaitForSeconds(typingSpeed);
                }
                lettersPassed++;
            }
            contSig.SetActive(true);

            isTyping = false;
        }

        else
        {
            CloseDialog();
            scentenceIndex = -1;
        }
    }

    private void CloseDialog()
    { // closes the dialog box and resets the dialog text

        dialogBox.SetActive(false);
        playingDialog = false;
        dialogText.text = "";
        scentenceIndex = -1;

        if (joystick != null)
        {
            joystick.SetActive(true);
        }

    }

    private void DisplayChoices(List<string> currentChoices)
    { // shows all the options and sets their texts, also hides the unused obejects


        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("There are more choices than UI spaces avaible: " + (currentChoices.Count - choices.Length));
        }

        int index = 0;
        foreach (string choice in currentChoices)
        {

            choices[index].gameObject.SetActive(true);
            choicesTxt[index].text = choice;

            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

    }
    public void MakeChoice(int id)
    {
        SoundMannager.Instance.PlaySound(clip[3]);
        scentenceIndex = linesForCall[id];
        StartCoroutine(DialogUpdate());
    }

    private void PlaySpeakFx()
    {
        SoundMannager.Instance.PlaySound(clip[AudioIndex]);
    }
}
