using UnityEngine;
using UnityEngine.UI;

public class DocksBookController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 2f;

    public int interactionCount;

    public DialogueController dialogueController;
    public Dialogue DocksBookDialogue1;
    public Dialogue DocksBookDialogue2;

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
                    if (sceneID == "Laudos4")
                    {
                        dialogueController.StartDialogue(DocksBookDialogue1);
                        updateInteractionCount(1);
                    }
                }
                else if (interactionCount >= 1)
                {
                    if (sceneID == "Laudos4")
                    {
                        dialogueController.StartDialogue(DocksBookDialogue2);
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
