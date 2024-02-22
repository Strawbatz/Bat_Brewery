using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI charName;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image portrait;
    [SerializeField] InputActionReference interact;
    [SerializeField] private float typeSpeed = 5;
    private float maxTypeTime = 0.2f;

    private Story currentStory;
    private bool dialogueIsPlaying;

    private Coroutine typeDialogueCorutine;
    private bool isTyping;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than 1 Dialogue Manager in scene!");
        }
        instance = this;
    }

    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    public void EnterDialogueMode(TextAsset inkJSON, TalkableNPC npc) {
        if(!dialogueIsPlaying) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        portrait.sprite = npc.portrait;
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
            }
            else {
                FinishParagraphEarly();
            }
        } 
        else if(!currentStory.canContinue && isTyping) {
            FinishParagraphEarly();
        } 
        else if(!isTyping){
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        portrait.sprite = null;
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

    public static DialogueManager GetInstance(){
        return instance;
    }
}
