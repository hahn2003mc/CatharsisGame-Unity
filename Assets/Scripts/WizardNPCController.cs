using UnityEngine;
using UnityEngine.UI;

public class WizardNPCController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;
    public DialogueController3 dialogueController3;
    public Dialogue CatharinAndWizardDialogue1;
    public Dialogue CatharinAndWizardDialogue2;

    public GameControllerPirateShip GameControllerPirateShip;

    public bool canInteract = true;

    public string sceneID;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionCount = 0;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance && canInteract)
            {
                if (interactionCount == 0)
                {
                    if (sceneID == "GrassWorld2")
                    {
                        dialogueController.StartDialogue(CatharinAndWizardDialogue1);
                    }
                    if (sceneID == "PirateShip3")
                    {
                        dialogueController3.StartDialogue(CatharinAndWizardDialogue2);
                    }
                }
                else if (interactionCount == 1)
                {
                    if (sceneID == "PirateShip3") { 
                    GameControllerPirateShip.finishScene();
                }
                }
            }
        }
    }

    public void updateInteractionCount(int value)
    {
        interactionCount = value;
    }

    public void setCanInteract(bool value)
    {
        canInteract = value;
    }

}
