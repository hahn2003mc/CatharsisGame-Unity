using UnityEngine;
using UnityEngine.UI;

public class GenericDialogueObjectController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 2f;

    public int interactionCount;

    public DialogueController dialogueController;
    public Dialogue dialogue0;
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    public Dialogue dialogue3;
    public Dialogue dialogue4;
    public Dialogue dialogue5;
    public Dialogue dialogue6;
    public Dialogue dialogue7;
    public Dialogue dialogue8;
    public Dialogue dialogue9;

    public bool increment;


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
                switch (interactionCount) {
                    case 0:
                        dialogueController.StartDialogue(dialogue0);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 1:
                        if (dialogue1 == null) { return;  }
                        dialogueController.StartDialogue(dialogue1);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 2:
                        if (dialogue2 == null) { return; }
                        dialogueController.StartDialogue(dialogue2);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 3:
                        if (dialogue3 == null) { return; }
                        dialogueController.StartDialogue(dialogue3);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 4:
                        if (dialogue4 == null) { return; }
                        dialogueController.StartDialogue(dialogue4);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 5:
                        if (dialogue5 == null) { return; }
                        dialogueController.StartDialogue(dialogue5);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 6:
                        if (dialogue6 == null) { return; }
                        dialogueController.StartDialogue(dialogue6);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 7:
                        if (dialogue7 == null) { return; }
                        dialogueController.StartDialogue(dialogue7);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 8:
                        if (dialogue8 == null) { return; }
                        dialogueController.StartDialogue(dialogue8);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    case 9:
                        if (dialogue9 == null) { return; }
                        dialogueController.StartDialogue(dialogue9);
                        if (increment)
                        {
                            updateInteractionCount(interactionCount + 1);
                        }
                        break;
                    default:
                        break;
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
