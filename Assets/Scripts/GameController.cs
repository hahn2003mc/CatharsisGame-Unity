using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public UserDataManager userDataManager;
    private string username;
    private string baseUrl = "";

    public PlayerController playerController;
    public KnightController knightController;
    public WizardController wizardController;
    public KnightFormController knightFormController;

    public GameObject knight;
    public GameObject wizard;
    public GameObject knightUI;
    public GameObject wizardUI;

    public GameObject skeletonPrefab;
    public GameObject skeletonGuard;
    
    public GruntController gruntController; // for skeleton reference
    public float playerVisibilityRange = 10f;

    public Transform[] spawnPositions;

    public GameObject areaCampFire;
    public GameObject areaCampFire2;

    public DragonController dragonController;

    public GameObject DeathScreen;

    public string keycode;

    private bool skeletonSpawning = false;
    private bool isDragonDefeated = false;

    public float skeletonSpawnCooldownLowerBound = 5f;
    public float skeletonSpawnCooldownUpperBound = 5f;

    public GameObject Portal;

    public int sceneCode = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        username = userDataManager.username;
        StartCoroutine(PostToAPI(username, APIEndpoints.createNewAccountTemplate, "createNewAccount"));

        // Debug.Log("username: " + username + ", baseUrl: " + baseUrl);
        knight.SetActive(true);
        wizard.SetActive(false);
        knightUI.SetActive(true);
        wizardUI.SetActive(false);
        knightFormController.SetForm(KnightFormController.KnightForm.Armor);
        knightFormController.LockForm(true);
        knightController.setCanAttack(true);
        knightController.canMove = true;
        wizardController.canMove = true;
        DeathScreen.SetActive(false);
        keycode = "0000";
        skeletonSpawning = false;
        isDragonDefeated = false;

        skeletonGuard.SetActive(true);
        Portal.SetActive(false);

        playerController.setCanSwapCharactersTrue();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHealth();
        if (!isDragonDefeated)
        {
            CheckDragonHealth();
        }
    }

    private IEnumerator PostToAPI(string username, string jsonBody, string operation) {
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
        else if (operation == "updateEnemiesCounts") {
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

    public void ConfigureEnemyCountsToUpdateAPI(string enemyName) {
        string jsonBody = APIEndpoints.updateEnemiesCountsTemplate;
        jsonBody = jsonBody.Replace("INPUT_ENEMY_NAME", enemyName);
        StartCoroutine(PostToAPI(username, jsonBody, "updateEnemiesCounts"));
    }

    public void StartSkeletonSpawning()
    {
        if (!skeletonSpawning)
        {
            StartCoroutine(RandomSkeletonSpawn());
            skeletonSpawning = true;
        }
    }

    private IEnumerator RandomSkeletonSpawn()
    {
        Debug.Log("in random spawn");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(skeletonSpawnCooldownLowerBound, skeletonSpawnCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            Debug.Log("about to spawn");
            SpawnSkeleton();

        }
    }

    void SpawnSkeleton()
    {
        foreach (Transform spawnPosition in spawnPositions)
        {
            Debug.Log("checking spawn position: " + spawnPosition.position);
            if (Vector2.Distance(playerController.transform.position, spawnPosition.position) >= playerVisibilityRange && Vector2.Distance(playerController.transform.position, spawnPosition.position) <= gruntController.detectionRange)
            {
                Debug.Log("found spawn - spawning");
                // this spawn position is outside the player's visibility range and within the skeleton's detection range, so we can spawn skeletons here
                Instantiate(skeletonPrefab, spawnPosition.position, Quaternion.identity);
                //Instantiate(skeletonPrefab, spawnPosition.position, Quaternion.identity);
                //Instantiate(skeletonPrefab, spawnPosition.position, Quaternion.identity);
                break; // exit the loop after spawning skeletons
            }
        }
    }

    void CheckPlayerHealth() 
    {
        if (playerController.health <= 0) 
        {
            StartCoroutine(HandleDeath());
        }
    }

    void CheckDragonHealth() 
    {
        if (dragonController.health <= 0)
        {
            isDragonDefeated = true;
            skeletonSpawning = true;
        }
    }

    IEnumerator HandleDeath()
    {
        DeathScreen.SetActive(true);
        if (dragonController.health <= 0) {
            playerController.transform.position = areaCampFire2.transform.position;
            dragonController.gameObject.SetActive(false);
        }
        else { 
            playerController.transform.position = areaCampFire.transform.position;
            dragonController.health = dragonController.maxHealth;
            dragonController.gameObject.SetActive(true);
        }
        playerController.health = playerController.maxHealth;
        playerController.invincible = true;

        wizardController.currentMana = wizardController.maxMana;
        knightController.currentEnergy = knightController.maxEnergy;

        yield return new WaitForSeconds(5f);

        playerController.invincible = false;
        DeathScreen.SetActive(false);

    }
    public void FlowerTracker(int flowerNumber)
    {
        // this function keeps a list (keycode) of four numbers, the last four numbers that the player interacted with for the flowers
        // it is initialized with 0000
        // each time the player interacts with a flower, the number of the flower is added to the end of the list, and the first number is removed
        // if the list is 1234, then the player has interacted with the flowers in the correct order, and the puzzle will be completed

        // make sure that the current digit to be added is not the same as the previous, if not, can add:
        if (flowerNumber.ToString() != keycode.Substring(keycode.Length - 1))
        {
            keycode = keycode.Substring(1) + flowerNumber.ToString();
        }
        if (keycode == "1234")
        {
            Debug.Log("puzzle completed");
            skeletonSpawning = false;
            skeletonGuard.SetActive(false);
            Portal.SetActive(true);
        }
    }
}
