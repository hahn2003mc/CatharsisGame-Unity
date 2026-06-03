using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogueController3 : MonoBehaviour
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

    public GameControllerPirateShip GameControllerPirateShip;

    public bool canSpeak;


    // TODO: refactor this to be a part of DialogueController
    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null) { return; }

        playerController.linearVelocity = Vector2.zero;

        knightController = knight.GetComponent<KnightController>();
        wizardController = wizard.GetComponent<WizardController>();
        knightAnimator = knight.GetComponent<Animator>();
        wizardAnimator = wizard.GetComponent<Animator>();


        // Force stop movement completely
        knightController.isWalking = false;
        wizardController.isWalking = false;

        // Reset animator parameters
        knightAnimator.SetBool("isWalking", false);
        wizardAnimator.SetBool("isWalking", false);

        // Optional but strong reset:
        knightAnimator.ResetTrigger("Attack");
        knightAnimator.ResetTrigger("HeavyAttack");
        wizardAnimator.ResetTrigger("Attack");

        // Play idle AFTER clearing params
        knightAnimator.Play("Idle", 0, 0f);
        wizardAnimator.Play("Idle", 0, 0f);

        if (knightController != null)
        {
            knightController.setCanMoveFalse();
        }
        if (wizardController != null)
        {
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
                GameControllerPirateShip.canMove();
                GameControllerPirateShip.finishDialogueProcessing(g_dialogue);
                g_dialogue = null;
                currentIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // End dialogue immediately
            DialogueContainer.SetActive(false);
            GameControllerPirateShip.canMove();
            g_dialogue = null;
            currentIndex = 0;
        }
    }
}
