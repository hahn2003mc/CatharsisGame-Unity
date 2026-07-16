using UnityEngine;
using UnityEngine.UI;

public class ViennaController : MonoBehaviour
{

    public GameObject Player;
    public PlayerController playerController;
    public GameObject self;

    public GameControllerGrassWorld gameControllerGrassWorld;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;
    public DialogueController3 dialogueController3;
    public Dialogue CatharinAndViennaDialogue1;
    public Dialogue CatharinAndViennaDialogue2;

    public GameObject Knight;
    public GameObject Wizard;

    public string sceneID;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneID == "GrassWorld2")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
                {

                    if (interactionCount == 0)
                    {
                        dialogueController.StartDialogue(CatharinAndViennaDialogue1);
                    }

                    if (interactionCount == 1)
                    {
                        playerController.invincible = true;
                        StartCoroutine(gameControllerGrassWorld.FadeToBlack());
                    }

                }
            }
        }
        else if (sceneID == "PirateShip3")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
                {
                    if (interactionCount == 0)
                    {
                        dialogueController3.StartDialogue(CatharinAndViennaDialogue2);
                    }
                    if (interactionCount == 1)
                    {
                        // do nothing
                    }
                }
            }
        }
    }

    public void updateInteractionCount(int value)
    {
        interactionCount = value;
    }

}
