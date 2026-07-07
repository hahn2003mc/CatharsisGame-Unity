using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public PlayerController playerController;

    int sceneCode = 0;
    public string spawnPointIndicator;
    public bool enteredWizardsHouse;

    private string savepath;

    public static SaveManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player == null)
        {
            return;
        }

        playerController = player.GetComponent<PlayerController>();
        savepath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save()
    {
        SaveData data = new SaveData();
        sceneCode = ResolveSceneCodeFromCurrentScene();
        data.world = sceneCode;

        data.spawnPointIndicator = spawnPointIndicator;
        data.enteredWizardsHouse = enteredWizardsHouse;
        if (playerController != null)
        {
            GameObject player = GameObject.Find("Player");
            playerController = player.GetComponent<PlayerController>();
            data.x = playerController.transform.position.x;
            data.y = playerController.transform.position.y;
            data.z = playerController.transform.position.z;
            data.normalDamage = playerController.GetComponentInChildren<KnightController>(true).swordDamage;
            data.heavyDamage = playerController.GetComponentInChildren<KnightController>(true).heavyAttackDamage;
            data.spellDamage = playerController.GetComponentInChildren<WizardController>(true).spellDamage;
            data.spellSpeed = playerController.GetComponentInChildren<WizardController>(true).spellSpeed;
            data.spellLifetime = playerController.GetComponentInChildren<WizardController>(true).spellLifetime;
        }

        string json = JsonUtility.ToJson(data);

        Debug.Log("Saving to disk...");
        File.WriteAllText(savepath, json);
    }

    public void Load() {
        if (!File.Exists(savepath))
        {
            return;
        }

        Debug.Log("Loading from disk...");

        string json = File.ReadAllText(savepath);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        spawnPointIndicator = data.spawnPointIndicator;
        enteredWizardsHouse = data.enteredWizardsHouse;
        if (playerController != null)
        {
            GameObject player = GameObject.Find("Player");
            playerController = player.GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(data.x, data.y, data.z);
            playerController.GetComponentInChildren<KnightController>(true).swordDamage = data.normalDamage;
            playerController.GetComponentInChildren<KnightController>(true).heavyAttackDamage = data.heavyDamage;
            playerController.GetComponentInChildren<WizardController>(true).spellDamage = data.spellDamage;
            playerController.GetComponentInChildren<WizardController>(true).spellSpeed = data.spellSpeed;
            playerController.GetComponentInChildren<WizardController>(true).spellLifetime = data.spellLifetime;
        }
    }

    private int ResolveSceneCodeFromCurrentScene()
    {
        GameObject gameController = GameObject.Find("GameController");

        if (gameController == null)
            return 0;

        var gc = gameController.GetComponent<GameController>();
        if (gc != null)
            return gc.sceneCode;

        var gcGrass = gameController.GetComponent<GameControllerGrassWorld>();
        if (gcGrass != null)
            return gcGrass.sceneCode;

        var gcPirate = gameController.GetComponent<GameControllerPirateShip>();
        if (gcPirate != null)
            return gcPirate.sceneCode;

        var gcLaudos = gameController.GetComponent<GameControllerLaudos>();
        if (gcLaudos != null)
            return gcLaudos.sceneCode;

        var gcHouse = gameController.GetComponent<GameControllerLaudosWizardsHouse>();
        if (gcHouse != null)
            return gcHouse.sceneCode;

        return 0;
    }
}
