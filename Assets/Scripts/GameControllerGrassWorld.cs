using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameControllerGrassWorld : MonoBehaviour
{
    public PlayerController playerController;
    public KnightController knightController;
    public GameObject knight;
    public WizardController wizardController;
    public GameObject wizard;
    public KnightFormController knightFormController;

    public UserDataManager userDataManager;
    private string username;
    private string baseUrl = "";

    public DoorController doorController;

    public GameObject bed;

    public GameObject knightSword;

    public GameObject KnightUI;
    public GameObject WizardUI;
    public GameObject ManaBarUI;
    public GameObject EnergyBarUI;

    public Dialogue CatharinMonologue1;

    public DialogueController dialogueController;

    public int interactionCount = 0;

    public bool inside;

    public GameObject LeafParticleSystem;

    public GameObject CatharinsDad;
    public DadController dadController;

    // spiders 
    public Transform[] spiderSpawnPositions;

    public bool spiderSpawning = false;

    public float spiderSpawnCooldownLowerBound = 50f;
    public float spiderSpawnCooldownUpperBound = 100f;

    public GruntController gruntController; // for skeleton reference
    public float playerVisibilityRange = 15f;

    public GameObject spiderPrefab;

    public GameObject spawnBarrier;
    public GameObject forestBarrier;

    public GameObject areaCampFire;
    public GameObject areaCampFire2;
    public GameObject DeathScreen;

    public GameObject wizardNPC;

    public string keycode = "00";

    public bool puzzleCompleted;

    public GameObject MotherSpider;
    public MotherSpiderController motherSpiderController;
    public bool spiderPrereq1Completed = false;
    public bool spiderPrereq2Completed = false;

    public bool isSpiderDefeated = false;

    public GameObject fogTilemap;

    public GameObject Sedna;
    public GameObject Vienna;
    public ViennaController viennaController;

    public GameObject FadePanel;
    public float fadeTime = 0.6f;

    public int sceneCode = 2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        username = userDataManager.username;
        StartCoroutine(PostToAPI(username, APIEndpoints.createNewAccountTemplate, "createNewAccount"));

        playerController.transform.position = new Vector3(bed.transform.position.x, bed.transform.position.y - 1, playerController.transform.position.z);
        knightFormController.SetForm(KnightFormController.KnightForm.Girl);
        knightFormController.LockForm(true);
        playerController.setCanSwapCharactersFalse();
        knightController.setCanAttack(false);
        knightSword.SetActive(false);
        wizard.SetActive(false);
        knight.SetActive(true);
        KnightUI.SetActive(false);
        WizardUI.SetActive(false);
        knightController.canMove = false;
        wizardController.canMove = false;
        inside = true;
        LeafParticleSystem.SetActive(false);
        wizardNPC.SetActive(true);
        wizardNPC.transform.position = new Vector3(9.7f, 75.5f, wizardNPC.transform.position.z);
        ManaBarUI.SetActive(false);
        EnergyBarUI.SetActive(false);
        MotherSpider.SetActive(false);
        dialogueController.StartDialogue(CatharinMonologue1);
        dadController = CatharinsDad.GetComponent<DadController>();
        spawnBarrier.SetActive(true);
        forestBarrier.SetActive(true);
        keycode = "00";
        puzzleCompleted = false;
        fogTilemap.SetActive(true);
        Sedna.SetActive(true);
        Vienna.SetActive(false);
        isSpiderDefeated = false;
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
        if (!puzzleCompleted)
        {
            playerController.transform.position = areaCampFire.transform.position;
        }
        else
        {
            playerController.transform.position = areaCampFire2.transform.position;
            if (motherSpiderController.health > 0)
            {
                motherSpiderController.health = motherSpiderController.maxHealth;
            }
        }
        playerController.health = playerController.maxHealth;
        playerController.invincible = true;

        wizardController.currentMana = wizardController.maxMana;
        knightController.currentEnergy = knightController.maxEnergy;

        yield return new WaitForSeconds(5f);

        playerController.invincible = false;
        DeathScreen.SetActive(false);

    }

    public void ConfigureEnemyCountsToUpdateAPI(string enemyName)
    {
        string jsonBody = APIEndpoints.updateEnemiesCountsTemplate;
        jsonBody = jsonBody.Replace("INPUT_ENEMY_NAME", enemyName);
        StartCoroutine(PostToAPI(username, jsonBody, "updateEnemiesCounts"));
    }

    private IEnumerator PostToAPI(string username, string jsonBody, string operation)
    {
        jsonBody = jsonBody.Replace("INPUT_USERNAME", username);
        Debug.Log("Posting json message: " + jsonBody + " to API...");
        byte[] rawBody = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = null;
        if (operation == "createNewAccount")
        {
            request = new UnityWebRequest(
                APIEndpoints.createNewAccount,
                "POST"
            );
        }
        else if (operation == "updateEnemiesCounts")
        {
            request = new UnityWebRequest(
                APIEndpoints.updateEnemiesCounts,
                "POST"
            );
        }
        if (request == null)
        {
            Debug.Log("invalid API operation");
            yield break;
        }
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    public void StartSpiderSpawning()
    {
        if (!spiderSpawning)
        {
            spiderSpawning = true;
        }
        StartCoroutine(RandomSpiderSpawn());
    }

    private IEnumerator RandomSpiderSpawn()
    {
        //Debug.Log("in random spawn");
        while (spiderSpawning)
        {

            // Wait a random cooldown
            float waitTime = UnityEngine.Random.Range(spiderSpawnCooldownLowerBound, spiderSpawnCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("about to spawn");
            SpawnSpider();

        }
    }

    void SpawnSpider()
    {
        foreach (Transform spawnPosition in spiderSpawnPositions)
        {
            //Debug.Log("checking spawn position: " + spawnPosition.position);
            if (Vector3.Distance(playerController.transform.position, spawnPosition.position) >= playerVisibilityRange && Vector3.Distance(playerController.transform.position, spawnPosition.position) <= gruntController.detectionRange)
            {
                //Debug.Log("found spawn - spawning");
                // this spawn position is outside the player's visibility range and within the skeleton's detection range, so we can spawn skeletons here
                Instantiate(spiderPrefab, spawnPosition.position, Quaternion.identity);
                //Instantiate(skeletonPrefab, spawnPosition.position, Quaternion.identity);
                //Instantiate(skeletonPrefab, spawnPosition.position, Quaternion.identity);
                break; // exit the loop after spawning skeletons
            }
        }
    }

    // called after dialogue finishes iterating
    public void canMove() {
        knightController.canMove = true;
        wizardController.canMove = true;
    }

    public void finishDialogueProcessing(Dialogue dialogue) {
        if (dialogue.name == "CatharinMonologue1")
        {
            knightController.setCanAttack(false);
        }
        else if (dialogue.name == "CatharinAndDadDialogue1")
        {
            knightController.setCanAttack(false);
            doorController.canLeave = true;
            CatharinsDad.SetActive(false);
            CatharinsDad.transform.position = new Vector3(36f, 55.5f, CatharinsDad.transform.position.z);
            CatharinsDad.SetActive(true);
            dadController.updateInteractionCount(1);
        }
        else if (dialogue.name == "CatharinAndDadDialogue2")
        {
            knightController.setCanAttack(false);
            CatharinsDad.SetActive(false);
            CatharinsDad.transform.position = new Vector3(54f, 52f, CatharinsDad.transform.position.z);
            CatharinsDad.SetActive(true);
            dadController.updateInteractionCount(2);
        }
        else if (dialogue.name == "CatharinAndDadDialogue3")
        {
            CatharinsDad.SetActive(false);
            dadController.updateInteractionCount(3);
            knightController.setCanAttack(true);
            knightFormController.LockForm(false);
            StartSpiderSpawning();
            spawnBarrier.SetActive(false);
            EnergyBarUI.SetActive(true);
            knightController.setCanAttack(true);
            playerController.setCanSwapCharactersFalse();
            playerController.setCanSwapCharactersFalse();
            playerController.setCanSwapCharactersFalse();
        }
        else if (dialogue.name == "CatharinAndSednaDialogue1") 
        {
            spiderPrereq1Completed = true;
        }
        else if (dialogue.name == "CatharinAndWizardDialogue1")
        {
            wizardNPC.SetActive(false);
            playerController.setCanSwapCharactersTrue();
            ManaBarUI.SetActive(true);
            spiderPrereq2Completed = true;
        }
        else if (dialogue.name == "CatharinAndViennaDialogue1")
        {
            viennaController.updateInteractionCount(1);
            Sedna.SetActive(true);
        }

    }

    public void motherSpiderDefeated() {
        isSpiderDefeated = true;
        spiderSpawning = false;
        fogTilemap.SetActive(false);
        Vienna.SetActive(true);
        playerController.setCanSwapCharactersFalse();
        knight.SetActive(true);
        wizard.SetActive(false);
        wizardNPC.SetActive(true);
        wizardNPC.transform.position = new Vector3(18f, 80.5f, wizardNPC.transform.position.z);
        KnightUI.SetActive(false);
        WizardUI.SetActive(false);
        ManaBarUI.SetActive(false);
        Sedna.SetActive(false);
        wizardNPC.GetComponent<WizardNPCController>().setCanInteract(false);
    }

    public void updateInside() 
    {
        inside = !inside;
        if (!inside)
        {
            LeafParticleSystem.SetActive(true);
        }
        else 
        {
            LeafParticleSystem.SetActive(false);
        }
    }

    public void TreeTracker(string treeNumber)
    {
        // this function keeps a list (keycode) of 2 numbers, the last 2 numbers that the player interacted with for the trees
        // it is initialized with 00 --> this is in hexadecimal   0 1 2 3 4 5 6 7 8 9 A B C D E F
        // each time the player interacts with a tree, the number of the tree is added to the end of the list, and the first number is removed
        // if the list is 53 (5:15), then the player has interacted with the trees in the correct order, and the puzzle will be completed

        // make sure that the current digit to be added is not the same as the previous, if not, can add:
        if (spiderPrereq1Completed && spiderPrereq2Completed) { 
            if (treeNumber.ToString() != keycode.Substring(keycode.Length - 1))
            {
                keycode = keycode.Substring(1) + treeNumber.ToString();
            }
            if (keycode == "53")
            {
                Debug.Log("puzzle completed");
                // play sound
                puzzleCompleted = true;
                MotherSpider.SetActive(true);
                forestBarrier.SetActive(false);
            }
        }
    }

    public IEnumerator FadeToBlack()
    {
        Image img = FadePanel.GetComponent<Image>();

        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / fadeTime);

            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

            yield return null; // wait one frame
        }
    }

}
