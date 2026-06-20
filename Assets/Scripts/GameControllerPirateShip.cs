using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerPirateShip : MonoBehaviour
{
    public PlayerController playerController;
    public KnightController knightController;
    public GameObject knight;
    public WizardController wizardController;
    public WizardNPCController wizardNPCController;
    public GameObject wizard;
    public KnightFormController knightFormController;

    // public DoorController doorController;

    public GameObject knightSword;

    public GameObject KnightUI;
    public GameObject WizardUI;
    public GameObject ManaBarUI;
    public GameObject EnergyBarUI;

    public DoorController doorController;

    public DialogueController3 dialogueController3;
    public Dialogue CatharinAndWizardDialogue3;

    public bool downstairsDialogueCompleted = false;

    public GameObject mainCamera;

    // public bool inside;




    // public GruntController gruntController; // for skeleton reference

    public GameObject wizardNPC;

    public GameObject Vienna;
    public ViennaController viennaController;

    public GameObject FadePanel;
    public float fadeTime = 2f;

    public void canMove()
    {
        knightController.canMove = true;
        wizardController.canMove = true;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController.transform.position = new Vector3(21.9f, 79.8f, playerController.transform.position.z);
        knightFormController.SetForm(KnightFormController.KnightForm.Girl);
        knightFormController.LockForm(true);
        playerController.canSwapCharacters = false;
        knightController.canAttack = false;
        knightSword.SetActive(false);
        wizard.SetActive(false);
        knight.SetActive(true);
        KnightUI.SetActive(false);
        WizardUI.SetActive(false);
        wizardNPC.SetActive(true);
        wizardNPC.transform.position = new Vector3(27.78f, 81.5f, wizardNPC.transform.position.z);
        Vienna.transform.position = new Vector3(103.99f, 88.0f, wizardNPC.transform.position.z);
        ManaBarUI.SetActive(false);
        EnergyBarUI.SetActive(false);
        Vienna.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckPlayerHealth();
    }

    void CheckPlayerHealth()
    {
        if (playerController.health <= 0)
        {
            //StartCoroutine(HandleDeath());
        }
    }

    public void finishDialogueProcessing(Dialogue dialogue)
    {
        if (dialogue.name == "CatharinAndWizardDialogue2")
        {
            doorController.canLeave = true;
            wizardNPCController.updateInteractionCount(1);
        }
        else if (dialogue.name == "CatharinAndViennaDialogue2")
        {
            downstairsDialogueCompleted = true;
        }
        else if (dialogue.name == "CatharinAndWizardDialogue3")
        {
            SceneManager.LoadScene("Laudos4");
        }
    }

    public void finishScene()
    {
        if (downstairsDialogueCompleted)
        {
            StartCoroutine(WaitAndStopMove());
            StartCoroutine(FadeToBlack());
            StartCoroutine(WaitAndStartDialogue());
        }
    }


    public IEnumerator FadeToBlack()
    {
        Image img = FadePanel.GetComponent<Image>();

        float elapsed = 0f;

        knightController.lockCameraToPlayer = false;

        Vector3 startPos = mainCamera.transform.position;

        // How far upward the camera should pan
        Vector3 targetPos = startPos + new Vector3(0f, 5f, 0f);

        Color startColor = img.color;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / fadeTime);

            // Smooth easing
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Fade UI
            img.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                smoothT
            );

            // Smooth camera pan
            mainCamera.transform.position = Vector3.Lerp(
                startPos,
                targetPos,
                smoothT
            );

            yield return null;
        }

        // Ensure final values are exact
        img.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        mainCamera.transform.position = targetPos;
    }

    public IEnumerator WaitAndStopMove() { 
        yield return new WaitForSeconds(1f);
        knightController.canMove = false;
    }

    public IEnumerator WaitAndStartDialogue()
    {
        yield return new WaitForSeconds(5f);
        dialogueController3.StartDialogue(CatharinAndWizardDialogue3);
    }

}
