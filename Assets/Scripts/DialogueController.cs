using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour 
{
    public GameObject DialogueContainer;
    public GameObject BackPanel;
    public GameObject DialogueBox;
    public GameObject DialogueText;
    public GameObject SpeakerText;
    public GameObject Portrait;

    public Rigidbody2D playerController;
    public GameObject knight;
    public GameObject wizard;
    public KnightController knightController;
    public WizardController wizardController;
    public Animator knightAnimator;
    public Animator wizardAnimator;

    public Dialogue g_dialogue;
    public int currentIndex;

    public GameControllerGrassWorld GameControllerGrassWorld;
    public GameControllerLaudos GameControllerLaudos;
    public GameControllerLaudosWizardsHouse GameControllerLaudosWizardsHouse;
    public string sceneID;

    public bool canSpeak;

    public void StartDialogue(Dialogue dialogue) {
        if (dialogue == null) { return; }

        if (playerController != null)
        {
            playerController.linearVelocity = Vector2.zero;

            knightController = knight.GetComponent<KnightController>();
            wizardController = wizard.GetComponent<WizardController>();
            knightAnimator = knight.GetComponent<Animator>();
            wizardAnimator = wizard.GetComponent<Animator>();


            // Force stop movement completely
            knightController.isWalking = false;
            knightController.setCanAttack(false);
            wizardController.isWalking = false;

            // Reset animator parameters
            knightAnimator.SetBool("isWalking", false);
            wizardAnimator.SetBool("isWalking", false);
            knightAnimator.ResetTrigger("Attack");
            knightAnimator.ResetTrigger("HeavyAttack");
            wizardAnimator.ResetTrigger("Attack");

            // Play idle AFTER clearing params
            knightAnimator.Play("Idle", 0, 0f);
            wizardAnimator.Play("Idle", 0, 0f);
        }

        if (knightController != null)
        {
            knightController.setCanMoveFalse();
        }
        if (wizardController != null) { 
            wizardController.setCanMoveFalse();
        }

        currentIndex = 0;

        g_dialogue = dialogue;

        DialogueContainer.SetActive(true);
        DialogueBox.SetActive(true);
        SpeakerText.SetActive(true);
        Portrait.SetActive(true);
        DialogueText.SetActive(true);
        BackPanel.SetActive(true);

        DialogueLine line0 = dialogue.lines[0];
        DialogueText.GetComponent<TextMeshProUGUI>().text = line0.text;
        SpeakerText.GetComponent<TextMeshProUGUI>().text = line0.speakerName;
        Portrait.GetComponent<Image>().sprite = line0.portrait;
        canSpeak = false;
        StartCoroutine(PauseDialogue());
        currentIndex++;

    }

    public IEnumerator PauseDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        canSpeak = true;
    }

    public void Update()
    {
        if (g_dialogue == null) { return; }
        int length = g_dialogue.lines.Length;

        if (sceneID == "GrassWorld2") { 
            if ((Input.GetKeyDown(KeyCode.Space)) && canSpeak)
            {
                //Debug.Log("Inside input function and length is " + length);
                //Debug.Log("current index is " + currentIndex);
                if (currentIndex < length)
                {
                    DialogueLine line = g_dialogue.lines[currentIndex];
                    DialogueText.GetComponent<TextMeshProUGUI>().text = line.text;
                    SpeakerText.GetComponent<TextMeshProUGUI>().text = line.speakerName;
                    Portrait.GetComponent<Image>().sprite = line.portrait;
                    canSpeak = false;
                    StartCoroutine(PauseDialogue());
                    currentIndex++;
                    //Debug.Log("current index after increment is " + currentIndex);
                }
                else
                {
                    // End of dialogue
                    DialogueContainer.SetActive(false);
                    GameControllerGrassWorld.canMove();
                    currentIndex = 0;
                    knightController.setCanAttack(true);
                    GameControllerGrassWorld.finishDialogueProcessing(g_dialogue);
                    g_dialogue = null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // End dialogue immediately
                DialogueContainer.SetActive(false);
                GameControllerGrassWorld.canMove();
                g_dialogue = null;
                currentIndex = 0;
                knightController.setCanAttack(true);
            }
        }
    
        else if (sceneID == "Laudos4") { 
            if ((Input.GetKeyDown(KeyCode.Space)) && canSpeak)
            {
                //Debug.Log("Inside input function and length is " + length);
                //Debug.Log("current index is " + currentIndex);
                if (currentIndex<length)
                {
                    DialogueLine line = g_dialogue.lines[currentIndex];
                    DialogueText.GetComponent<TextMeshProUGUI>().text = line.text;
                    SpeakerText.GetComponent<TextMeshProUGUI>().text = line.speakerName;
                    Portrait.GetComponent<Image>().sprite = line.portrait;
                    canSpeak = false;
                    StartCoroutine(PauseDialogue());
                    currentIndex++;
                    //Debug.Log("current index after increment is " + currentIndex);
                }
                else
                {
                    // End of dialogue
                    DialogueContainer.SetActive(false);
                    GameControllerLaudos.canMove();
                    currentIndex = 0;
                    knightController.setCanAttack(true);
                    GameControllerLaudos.finishDialogueProcessing(g_dialogue);
                    g_dialogue = null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // End dialogue immediately
                DialogueContainer.SetActive(false);
                GameControllerLaudos.canMove();
                g_dialogue = null;
                currentIndex = 0;
                knightController.setCanAttack(true);
            }
        }

        else if (sceneID == "Laudos4WizardsHouse")
        {
            if ((Input.GetKeyDown(KeyCode.Space)) && canSpeak)
            {
                //Debug.Log("Inside input function and length is " + length);
                //Debug.Log("current index is " + currentIndex);
                if (currentIndex < length)
                {
                    DialogueLine line = g_dialogue.lines[currentIndex];
                    DialogueText.GetComponent<TextMeshProUGUI>().text = line.text;
                    SpeakerText.GetComponent<TextMeshProUGUI>().text = line.speakerName;
                    Portrait.GetComponent<Image>().sprite = line.portrait;
                    canSpeak = false;
                    StartCoroutine(PauseDialogue());
                    currentIndex++;
                    //Debug.Log("current index after increment is " + currentIndex);
                }
                else
                {
                    // End of dialogue
                    DialogueContainer.SetActive(false);
                    GameControllerLaudosWizardsHouse.finishDialogueProcessing(g_dialogue);
                    g_dialogue = null;
                    currentIndex = 0;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                // End dialogue immediately
                DialogueContainer.SetActive(false);
                g_dialogue = null;
                currentIndex = 0;
            }
        }
    }
            
}
