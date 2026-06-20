using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerLaudosWizardsHouse : MonoBehaviour
{
    public GameObject rock;
    public GameObject rockBorder;
    public GameObject rockTwist;
    public GameObject cup;
    public GameObject cupBorder;
    public GameObject oracle;
    public GameObject oracleBorder;
    public GameObject oracle6;

    public GameObject AKeyMarker;
    public GameObject SKeyMarker;
    public GameObject DKeyMarker;

    public bool canInteract;
    public bool rockInteracted = false;
    public bool cupInteracted = false;
    public bool oracleInteracted = false;

    public DialogueController dialogueController;

    public Dialogue CatharinAndWizardDialogue4;
    public Dialogue CatharinAndWizardManaStone1;
    public Dialogue CatharinAndWizardManaStone2;
    public Dialogue CatharinAndWizardManaStone3;
    public Dialogue CatharinAndWizardManaStone4;
    public Dialogue CatharinAndWizardCup;
    public Dialogue CatharinAndWizardOracle1;
    public Dialogue CatharinAndWizardOracle2;
    public Dialogue CatharinAndWizardOracle3;
    public Dialogue CatharinAndWizardDialogue5;

    public GameObject mainCamera;

    public GameObject FadePanel;
    public float fadeTime = 2f;

    public int sceneCode = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // pauseMovement();
        // dialogueController.StartDialogue(CatharinAndViennaDialogue3);

        canInteract = false;
        rock.SetActive(true);
        rockTwist.SetActive(false);
        cup.SetActive(true);
        oracle.SetActive(true);
        rockBorder.SetActive(false);
        cupBorder.SetActive(false);
        oracleBorder.SetActive(false);
        AKeyMarker.SetActive(false);
        SKeyMarker.SetActive(false);
        DKeyMarker.SetActive(false);
        oracle6.SetActive(false);
        dialogueController.StartDialogue(CatharinAndWizardDialogue4);
    }

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.A) && !rockInteracted)
            {
                canInteract = false;
                cupBorder.SetActive(false);
                cup.SetActive(true);
                oracleBorder.SetActive(false);
                oracle.SetActive(true);
                AKeyMarker.SetActive(false);
                SKeyMarker.SetActive(false);
                DKeyMarker.SetActive(false);
                StartCoroutine(rockObjectProcessing());
            }
            else if (Input.GetKeyDown(KeyCode.S) && !cupInteracted)
            {
                canInteract = false;
                rockBorder.SetActive(false);
                rock.SetActive(true);
                oracleBorder.SetActive(false);
                oracle.SetActive(true);
                AKeyMarker.SetActive(false);
                SKeyMarker.SetActive(false);
                DKeyMarker.SetActive(false);
                StartCoroutine(cupObjectProcessing());
            }
            else if (Input.GetKeyDown(KeyCode.D) && !oracleInteracted)
            {
                canInteract = false;
                cupBorder.SetActive(false);
                cup.SetActive(true);
                rockBorder.SetActive(false);
                rock.SetActive(true);
                AKeyMarker.SetActive(false);
                SKeyMarker.SetActive(false);
                DKeyMarker.SetActive(false);
                StartCoroutine(oracleObjectProcessing());
            }
        }
        if (canInteract && rockInteracted && cupInteracted && oracleInteracted) {
            canInteract = false;
            StartCoroutine(WaitAndStartDialogue(CatharinAndWizardDialogue5, 1.5f));
        }
    }

    private IEnumerator rockObjectProcessing()
    {
        AKeyMarker.SetActive(false);
        rock.SetActive(true);
        rockBorder.SetActive(false);

        // Dialogue 1
        dialogueController.StartDialogue(CatharinAndWizardManaStone1);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        yield return new WaitForSeconds(1f);

        // Rock animation
        rock.SetActive(false);
        rockBorder.SetActive(false);
        rockTwist.SetActive(true);

        yield return new WaitForSeconds(2f);

        rockTwist.SetActive(false);
        rock.SetActive(true);

        yield return new WaitForSeconds(1f);

        // Dialogue 2
        dialogueController.StartDialogue(CatharinAndWizardManaStone2);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        yield return new WaitForSeconds(5f);

        // Dialogue 3
        dialogueController.StartDialogue(CatharinAndWizardManaStone3);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        AKeyMarker.SetActive(true);
        rockBorder.SetActive(true);
        rock.SetActive(false);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.A));
        AKeyMarker.SetActive(false);
        rockBorder.SetActive(false);
        rock.SetActive(true);

        // Dialogue 4
        dialogueController.StartDialogue(CatharinAndWizardManaStone4);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        rockInteracted = true;
        canInteract = true;

        if (!cupInteracted)
        {
            SKeyMarker.SetActive(true);
            cup.SetActive(false);
            cupBorder.SetActive(true);
        }
        if (!oracleInteracted)
        {
            DKeyMarker.SetActive(true);
            oracle.SetActive(false);
            oracleBorder.SetActive(true);
        }
    }

    private IEnumerator cupObjectProcessing() {
        SKeyMarker.SetActive(false);
        cup.SetActive(true);
        cupBorder.SetActive(false);

        dialogueController.StartDialogue(CatharinAndWizardCup);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        cupInteracted = true;
        canInteract = true;

        if (!rockInteracted)
        {
            AKeyMarker.SetActive(true);
            rock.SetActive(false);
            rockBorder.SetActive(true);
        }
        if (!oracleInteracted)
        {
            DKeyMarker.SetActive(true);
            oracle.SetActive(false);
            oracleBorder.SetActive(true);
        }
    }

    private IEnumerator oracleObjectProcessing() {
        DKeyMarker.SetActive(false);
        oracle.SetActive(true);
        oracleBorder.SetActive(false);

        dialogueController.StartDialogue(CatharinAndWizardOracle1);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        yield return new WaitForSeconds(5f);

        dialogueController.StartDialogue(CatharinAndWizardOracle2);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        yield return new WaitForSeconds(3f);

        oracle6.SetActive(true);
        yield return new WaitForSeconds(1f);
        oracle6.SetActive(false);

        dialogueController.StartDialogue(CatharinAndWizardOracle3);
        yield return new WaitUntil(() => dialogueController.g_dialogue == null);

        oracleInteracted = true;
        canInteract = true;

        if (!rockInteracted)
        {
            AKeyMarker.SetActive(true);
            rock.SetActive(false);
            rockBorder.SetActive(true);
        }
        if (!cupInteracted)
        {
            SKeyMarker.SetActive(true);
            cup.SetActive(false);
            cupBorder.SetActive(true);
        }
    }

    public void finishDialogueProcessing(Dialogue dialogue)
    {
        if (dialogue.name == "CatharinAndWizardDialogue4")
        {
            rock.SetActive(false);
            cup.SetActive(false);
            oracle.SetActive(false);
            rockBorder.SetActive(true);
            cupBorder.SetActive(true);
            oracleBorder.SetActive(true);
            AKeyMarker.SetActive(true);
            SKeyMarker.SetActive(true);
            DKeyMarker.SetActive(true);
            canInteract = true;
        }
        else if (dialogue.name == "CatharinAndWizardDialogue5") {
            transferToLaudosScene();
        }
    }

    public void transferToLaudosScene()
    {
        StartCoroutine(waitForFadeToFinish());
    }

    private IEnumerator waitForFadeToFinish()
    {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("Laudos4");
    }

    public void finishScene()
    {
        StartCoroutine(waitForFadeToFinish());
    }

    public IEnumerator WaitAndStartDialogue(Dialogue dialogue, float time)
    {
        yield return new WaitForSeconds(time);
        dialogueController.StartDialogue(dialogue);
    }


    public IEnumerator FadeToBlack()
    {
        Image img = FadePanel.GetComponent<Image>();

        float elapsed = 0f;

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

            yield return null;
        }

        // Ensure final values are exact
        img.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }

}
