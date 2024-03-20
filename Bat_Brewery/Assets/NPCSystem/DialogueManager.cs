using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using System;

/// <summary>
/// Singleton class handling dialogue from start to exit.
/// start dialogue by calling EnterDialogueMode(TextAsset inkJSON, TalkableNPC npc).
/// Will send out unity actions when dialogue is ended or a choice is picked
/// with an int indicating which choice was picked. 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [Header("UI containers")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject portrait1;
    [SerializeField] Image portrait1Image;
    [SerializeField] GameObject portrait2;
    [SerializeField] Image fabianImage;
    [SerializeField] GameObject choicesContainer;
    [SerializeField] GameObject firstChoice;
    [SerializeField] TextMeshProUGUI charName;
    [SerializeField] TextMeshProUGUI dialogueText;
    
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject inactiveChoices;
    [Header("Typing configuration")]
    [SerializeField] private float typeSpeed = 5;
    private float maxTypeTime = 0.2f;

    [Header("Fabian Portraits")]
    [SerializedDictionary("mood", "sprite")]
    [SerializeField] SerializedDictionary<string, Sprite> fabianMoods;
    private string currentMood = "normal";

    //Components  for progressing the story.
    private Story currentStory;
    private string currentStoryId;
    private TextMeshProUGUI[] choicesText;
    private Coroutine typeDialogueCorutine;

    //Bools tracking different states of the dialogue.
    public bool dialogueIsPlaying {get; private set;}
    private bool isTyping;
    private bool choicesPending;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than 1 Dialogue Manager in scene!");
        }
        instance = this;
    }

    /// <summary>
    /// On start disable all components and set up choicesText with UI components
    /// for choices. 
    /// </summary>
    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        portrait1.SetActive(false);
        portrait2.SetActive(false);
        choicesContainer.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index ++;
        }
    }

    /// <summary>
    /// Function for starting dialogue.
    /// This sets up a new story for the dialogue manager to go trough,
    /// sets relevant components as visible and assigns them content as well
    /// as signing up to listen for interaction input to continue story.
    /// </summary>
    /// <param name="inkJSON">Dialogue to be had</param>
    /// <param name="npc">NPC that is having the dialogue</param>
    public void EnterDialogueMode(TextAsset inkJSON, TalkableNPC npc) {
        if(dialogueIsPlaying) return;
        GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("dialogue", true);
        currentStory = new Story(inkJSON.text);
        currentStoryId = inkJSON.name;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portrait1.SetActive(true);
        portrait1Image.sprite = npc.portrait;
        portrait2.SetActive(true);
        charName.text = npc.npcName;
        GameEventsManager.instance.dialogueEvents.DialogueStarted(currentStoryId);
        GameEventsManager.instance.inputEvents.onPlayerInteracted += ContinueStory;
        
        ContinueStory();
    }

    public void EnterDescription(TextAsset inkJSON) {
        if(dialogueIsPlaying) return;
        GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("dialogue", true);
        currentStory = new Story(inkJSON.text);
        currentStoryId = inkJSON.name;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portrait1.SetActive(false);
        charName.text = "";
        portrait2.SetActive(true);
        fabianImage.sprite = fabianMoods[currentMood];
        GameEventsManager.instance.inputEvents.onPlayerInteracted += ContinueStory;
        
        ContinueStory();
    }

    private void ContinueStory(InputAction.CallbackContext ctx) {ContinueStory();}

    /// <summary>
    /// Function called when interact is pressed to enable continuation of story
    /// if suitable or exit dialogue when its done. It will not continue while
    /// text is typing or there is a choice to be made. If it is currently typing
    /// it will finish the paragraph early.
    /// </summary>
    private void ContinueStory() {
        if(currentStory.canContinue) {
            if(!isTyping) {
                typeDialogueCorutine = StartCoroutine(TypeDialogueText(currentStory.Continue()));
                UpdateMood();
                DisplayChoices();
            }
            else {
                FinishParagraphEarly();
            }
        } 
        else if(!currentStory.canContinue && isTyping) {
            FinishParagraphEarly();
        } 
        else if(!isTyping && !choicesPending){
            ExitDialogueMode();
        }
    }

    /// <summary>
    /// When exiting dialogue hide UI, stop listening to interaction input
    /// and send out an action saying dialogue has ended.
    /// </summary>
    private void ExitDialogueMode() {
        GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("dialogue", false);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        portrait1.SetActive(false);
        portrait2.SetActive(false);
        choicesContainer.SetActive(false);
        portrait1Image.sprite = null;
        currentMood = "normal";
        fabianImage.sprite = fabianMoods[currentMood];
        charName.text = "";
        dialogueText.text = "";

        GameEventsManager.instance.inputEvents.onPlayerInteracted -= ContinueStory;
        GameEventsManager.instance.dialogueEvents.DialogueEnded(currentStoryId);
    }

    /// <summary>
    /// Corutine for typing out given string one character at a time.
    /// </summary>
    /// <param name="p">String to be typed out.</param>
    /// <returns></returns>
    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        dialogueText.text = p;
        dialogueText.maxVisibleCharacters = maxVisibleChars;        

        foreach (char c in p.ToCharArray())
        {

            maxVisibleChars++;
            dialogueText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(maxTypeTime / typeSpeed);
        }

        isTyping = false;
    }

    /// <summary>
    /// Finishes a paragraph early bu canceling corutine and showing all
    /// text at once.
    /// </summary>
    private void FinishParagraphEarly() {
        StopCoroutine(typeDialogueCorutine);
        dialogueText.maxVisibleCharacters = 500;
        dialogueText.text = currentStory.currentText;
        isTyping = false;
    }

    /// <summary>
    /// If there are any choices to be made they will be displayed.
    /// It will only display as many options as story contains.
    /// </summary>
    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > choices.Length) Debug.Log("More choices given than UI supports");
        choicesContainer.SetActive(false);
        if(currentChoices.Count > 0) {
            choicesPending = true;
            int index = 0;
            foreach(Choice choice in currentChoices) {

                choices[index].SetActive(true);
                choices[index].transform.SetParent(choicesContainer.transform);
                choicesText[index].text = choice.text;
                index ++;
            }

            for (int i = index; i < choices.Length; i++) {
                choices[i].SetActive(false);
                choices[i].transform.SetParent(inactiveChoices.transform);
            }
            choicesContainer.SetActive(true);
            StartCoroutine(SelectFirstChoice());
        } else {
            choicesPending = false;
        }
    }

    /// <summary>
    /// When a choice is made, pick it for the story and send
    /// out a unity action about which choice was picked.
    /// </summary>
    /// <param name="choiceIndex">Choice picked</param>
    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
        GameEventsManager.instance.dialogueEvents.ChoiceMade(currentStoryId, choiceIndex);
        choicesPending = false;
        ContinueStory();
        if(dialogueText.text == "") {
            ExitDialogueMode();
        }
    }

    /// <summary>
    /// Update Fabians portrait based on mood in story.
    /// Defaults to normal mood if there is no mood set(or misspelled).
    /// </summary>
    private void UpdateMood() {
        if(currentStory.variablesState["mood"] != null) {
            string mood = (string)currentStory.variablesState["mood"];
            if(fabianMoods.ContainsKey(mood)) {
                currentMood = mood;
                fabianImage.sprite = fabianMoods[currentMood];
            } else {
                currentMood = "normal";
                fabianImage.sprite = fabianMoods[currentMood];
            }
        }
    }

    /// <summary>
    /// Corutine for selecting a first choice for navigaition
    /// with controller/keyboard. Waiting a frame between deselecting
    /// and selecting a new item for the eventsystem is needed because 
    /// unity is unity.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectFirstChoice(){
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstChoice);
    }

    public static DialogueManager GetInstance(){
        return instance;
    }
}
