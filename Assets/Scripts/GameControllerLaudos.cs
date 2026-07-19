using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerLaudos : MonoBehaviour
{
    public PlayerController playerController;
    public KnightController knightController;
    public GameObject knight;
    public WizardController wizardController;
    public WizardNPCController wizardNPCController;
    public GameObject wizard;
    public KnightFormController knightFormController;
    public Animator knightAnimator;
    public Animator wizardAnimator;

    public GameObject knightSword;

    public GameObject KnightUI;
    public GameObject WizardUI;
    public GameObject ManaBarUI;
    public GameObject EnergyBarUI;

    // public DoorController doorController;

    public DialogueController dialogueController;

    public Dialogue CatharinAndViennaDialogue3;
    public Dialogue CatharinAndBiscusDialogue2;
    public Dialogue CatharinAndKaelDialogue1;

    public GameObject mainCamera;

    public GameObject wizardNPC;

    public GameObject Vienna;
    public ViennaController viennaController;
    public GameObject pirateShip;

    public GameObject Biscus;
    public GameObject Kael;
    public BiscusController biscusController;

    public GameObject areaCampFire;
    public GameObject areaCampFire2;
    public GameObject DeathScreen;

    public GameObject FadePanel;
    public float fadeTime = 2f;

    public GameObject biscusInteractionCollider;
    public GameObject psychicSpellAnimationObject;

    public GameObject psychicPinkPanel;

    private SaveManager saveManager;

    public int sceneCode = 4;

    private void Awake()
    {
        saveManager = SaveManager.instance;
    }

    public void canMove()
    {
        knightController.canMove = true;
        wizardController.canMove = true;
    }

    public void saveToDisk() {
        saveManager.Save();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager.Load();
        Debug.Log(saveManager.spawnPointIndicator);
        Debug.Log(saveManager.enteredWizardsHouse);
        Debug.Log("Local Save Path: " + Application.persistentDataPath);
        switch (saveManager.spawnPointIndicator)
            {
            case "Docks":
                playerController.transform.position = new Vector3(22.5f, 83.8f, playerController.transform.position.z);
                knightFormController.SetForm(KnightFormController.KnightForm.Armor);
                knightFormController.LockForm(false);
                playerController.setCanSwapCharactersTrue();
                knightController.setCanAttack(true);
                knightSword.SetActive(true);
                wizard.SetActive(false);
                knight.SetActive(true);
                KnightUI.SetActive(false);
                WizardUI.SetActive(false);
                Vienna.transform.position = new Vector3(19, 82.0f, wizardNPC.transform.position.z);
                pirateShip.SetActive(true);
                ManaBarUI.SetActive(true);
                EnergyBarUI.SetActive(true);
                Vienna.SetActive(true);
                canMove();
                dialogueController.StartDialogue(CatharinAndViennaDialogue3);
                biscusInteractionCollider.SetActive(true);
                psychicSpellAnimationObject.SetActive(false);
                Kael.SetActive(false);

                saveManager.spawnPointIndicator = "Docks";
                saveToDisk();
                break;

            case "WizardsHouse":
                playerController.transform.position = new Vector3(56.1f, 145.0f, playerController.transform.position.z);
                knightFormController.SetForm(KnightFormController.KnightForm.Girl);
                knightFormController.LockForm(false);
                playerController.setCanSwapCharactersFalse();
                knightController.setCanAttack(false);
                knightSword.SetActive(false);
                wizard.SetActive(false);
                knight.SetActive(true);
                KnightUI.SetActive(false);
                WizardUI.SetActive(false);
                wizardNPC.SetActive(true);
                ManaBarUI.SetActive(false);
                EnergyBarUI.SetActive(true);
                Vienna.SetActive(false);
                pirateShip.SetActive(false);
                knightController.canMove = false;
                biscusInteractionCollider.SetActive(false);
                wizardNPC.transform.position = new Vector3(56.8f, 144.8f, playerController.transform.position.z);
                Kael.transform.position = new Vector3(55.1f, 145.5f, playerController.transform.position.z);
                Kael.SetActive(true);
                StartCoroutine(WaitAndChangeDirection(Vector2.down, 0f, 0f, -1f));
                StartCoroutine(WaitAndChangeDirection(Vector2.left, 2f, -1f, 0f));
                StartCoroutine(WaitAndStartDialogue(CatharinAndKaelDialogue1, 3f));
                
                saveManager.spawnPointIndicator = "WizardsHouse";
                saveManager.enteredWizardsHouse = true;
                saveToDisk();
                break;

            default:
                playerController.transform.position = new Vector3(22.5f, 83.8f, playerController.transform.position.z);
                knightFormController.SetForm(KnightFormController.KnightForm.Armor);
                knightFormController.LockForm(false);
                playerController.setCanSwapCharactersTrue();
                knightController.setCanAttack(true);
                knightSword.SetActive(true);
                wizard.SetActive(false);
                knight.SetActive(true);
                KnightUI.SetActive(false);
                WizardUI.SetActive(false);
                Vienna.transform.position = new Vector3(19, 82.0f, wizardNPC.transform.position.z);
                pirateShip.SetActive(true);
                ManaBarUI.SetActive(true);
                EnergyBarUI.SetActive(true);
                Vienna.SetActive(true);
                canMove();
                dialogueController.StartDialogue(CatharinAndViennaDialogue3);
                biscusInteractionCollider.SetActive(true);
                psychicSpellAnimationObject.SetActive(false);

                saveManager.spawnPointIndicator = "Docks";
                saveToDisk();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHealth();
    }

    void CheckPlayerHealth()
    {
        if (playerController.health <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        DeathScreen.SetActive(true);

        playerController.health = playerController.maxHealth;
        playerController.invincible = true;

        wizardController.currentMana = wizardController.maxMana;
        knightController.currentEnergy = knightController.maxEnergy;

        yield return new WaitForSeconds(5f);

        playerController.invincible = false;
        DeathScreen.SetActive(false);

    }

    public void finishDialogueProcessing(Dialogue dialogue)
    {
        if (dialogue.name == "CatharinAndBiscusDialogue1")
        {
            biscusController.updateInteractionCount(1);
            pauseMovement();
            applyBiscusEffects();
        }
        else if (dialogue.name == "CatharinAndKaelDialogue1") 
        {
            wizardNPC.SetActive(false);
            Kael.transform.position = new Vector3(-52.1f, 95.7f, playerController.transform.position.z);
        }
    }

    public void transferToWizardsHouseScene()
    {
        if (!saveManager.enteredWizardsHouse) { 
            saveManager.spawnPointIndicator = "WizardsHouse";
            saveManager.enteredWizardsHouse = true;
            saveToDisk();
            StartCoroutine(waitForFadeToFinish());
        }
    }

    private IEnumerator waitForFadeToFinish() {
        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene("Laudos4_WizardsHouse");
    }

    public void finishScene()
    {

        // StartCoroutine(FadeToBlack());

    }

    public void pauseMovement() {
        playerController.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

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
    }

    public void applyBiscusEffects() {
        // apply spell sprite
        psychicSpellAnimationObject.SetActive(true);
        psychicSpellAnimationObject.GetComponent<Animator>().Play("PsychicSpellAnimation");

        Invoke(nameof(DisablePsychicSpellAnimation), 1.0f);

        // flash pink screen object
        psychicPinkPanel.SetActive(true);
        StartCoroutine(FlashPsychicPanel());

        Debug.Log("Flashing panel, starting dialogue processing");
        StartCoroutine(WaitAndStartDialogue(CatharinAndBiscusDialogue2, 2f));
    }

    public void DisablePsychicSpellAnimation() {
        psychicSpellAnimationObject.SetActive(false);
    }

    public IEnumerator WaitAndStartDialogue(Dialogue dialogue, float time) {
        yield return new WaitForSeconds(time);
        dialogueController.StartDialogue(dialogue);
    }

    public IEnumerator WaitAndChangeDirection(Vector2 direction, float time)
    {
        yield return new WaitForSeconds(time);
        playerController.lastDirection = direction;
        knightController.ApplyFacingDirection();
        wizardController.ApplyFacingDirection();
    }

    public IEnumerator WaitAndChangeDirection(Vector2 direction, float time, float moveX, float moveY)
    {
        yield return new WaitForSeconds(time);
        playerController.lastDirection = direction;
        Animator knightAnimator = knightController.GetComponent<Animator>();
        knightAnimator.SetFloat("moveX", moveX);
        knightAnimator.SetFloat("moveY", moveY);
        knightController.ApplyFacingDirection();
        Animator wizardAnimator = wizardController.GetComponent<Animator>();
        wizardAnimator.SetFloat("moveX", moveX);
        wizardAnimator.SetFloat("moveY", moveY);
        wizardController.ApplyFacingDirection();
    }


    public IEnumerator FadeToBlack()
    {
        Image img = FadePanel.GetComponent<Image>();

        float elapsed = 0f;

        knightController.lockCameraToPlayer = false;

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



    public IEnumerator FlashPsychicPanel()
    {
        Image img = psychicPinkPanel.GetComponent<Image>();

        Color baseColor = img.color;

        // Start invisible
        img.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);

        float flashDuration = 0.15f;

        // Do two flashes
        for (int flash = 0; flash < 2; flash++)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / flashDuration);
                float smoothT = Mathf.SmoothStep(0f, 0.5f, t);

                img.color = new Color(
                    baseColor.r,
                    baseColor.g,
                    baseColor.b,
                    smoothT
                );

                yield return null;
            }

            // Fade out
            elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;

                float t = Mathf.Clamp01(elapsed / flashDuration);
                float smoothT = Mathf.SmoothStep(0f, 0.5f, t);

                img.color = new Color(
                    baseColor.r,
                    baseColor.g,
                    baseColor.b,
                    0.5f - smoothT
                );

                yield return null;
            }

            // Small pause between flashes
            if (flash == 0)
                yield return new WaitForSeconds(0.05f);
        }

        // Ensure invisible at end
        img.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
    }

}
