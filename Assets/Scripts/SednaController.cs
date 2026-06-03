using UnityEngine;
using UnityEngine.UI;

public class SednaController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;
    public Dialogue CatharinAndSednaDialogue1;
    public Dialogue CatharinAndSednaDialogue2;
    public Dialogue ArrinAndSednaDialogue1;

    public GameObject Knight;
    public GameObject Wizard;

    public GameControllerGrassWorld gameControllerGrassWorld;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
            {
                if (Knight.activeInHierarchy)
                {
                    if (!gameControllerGrassWorld.isSpiderDefeated)
                    {
                        dialogueController.StartDialogue(CatharinAndSednaDialogue1);
                    }
                    else if (gameControllerGrassWorld.isSpiderDefeated)
                    {
                        dialogueController.StartDialogue(CatharinAndSednaDialogue2);
                    }
                }
                else if (Wizard.activeInHierarchy)
                {
                     dialogueController.StartDialogue(ArrinAndSednaDialogue1);
                }
            }
        }
    }

    public void updateInteractionCount(int value)
    {
        interactionCount = value;
    }

}
