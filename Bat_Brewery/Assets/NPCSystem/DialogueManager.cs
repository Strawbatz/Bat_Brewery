using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject portrait1;
    [SerializeField] GameObject portrait2;
    [SerializeField] GameObject choicesContainer;
    [SerializeField] GameObject firstChoice;
    [SerializeField] TextMeshProUGUI charName;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image portrait1Image;
    [SerializeField] private GameObject[] choices;
    [SerializeField] InputActionReference interact;
    [SerializeField] private float typeSpeed = 5;
    private float maxTypeTime = 0.2f;

    private Story currentStory;
    private TextMeshProUGUI[] choicesText;
    private bool dialogueIsPlaying;

    private Coroutine typeDialogueCorutine;
    private bool isTyping;
    private bool choicesPending;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than 1 Dialogue Manager in scene!");
        }
        instance = this;
    }

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

    public void EnterDialogueMode(TextAsset inkJSON, TalkableNPC npc) {
        if(!dialogueIsPlaying) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portrait1.SetActive(true);
        portrait1Image.sprite = npc.portrait;
        charName.text = npc.NPCName;

        interact.action.performed += ContinueStory;
        
        ContinueStory();
        }
    }

    private void ContinueStory(InputAction.CallbackContext ctx) {ContinueStory();}

    private void ContinueStory() {
        if(currentStory.canContinue) {
            if(!isTyping) {
                typeDialogueCorutine = StartCoroutine(TypeDialogueText(currentStory.Continue()));
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

    private void ExitDialogueMode() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        portrait1.SetActive(false);
        choicesContainer.SetActive(false);
        portrait1Image.sprite = null;
        dialogueText.name = "";
        dialogueText.text = "";

        interact.action.performed -= ContinueStory;
    }

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

    private void FinishParagraphEarly() {
        StopCoroutine(typeDialogueCorutine);
        dialogueText.maxVisibleCharacters = 500;
        dialogueText.text = currentStory.currentText;
        isTyping = false;
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > choices.Length) Debug.Log("More choices given than UI supports");
        choicesContainer.SetActive(false);
        if(currentChoices.Count > 0) {
            choicesPending = true;
            int index = 0;
            foreach(Choice choice in currentChoices) {

                choices[index].SetActive(true);
                choicesText[index].text = choice.text;
                index ++;
            }

            for (int i = index; i < choices.Length; i++) {
                choices[i].SetActive(false);
            }
            choicesContainer.SetActive(true);
            StartCoroutine(SelectFirstChoice());
        } else {
            choicesPending = false;
        }
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private IEnumerator SelectFirstChoice(){
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstChoice);
    }

    public static DialogueManager GetInstance(){
        return instance;
    }
}
