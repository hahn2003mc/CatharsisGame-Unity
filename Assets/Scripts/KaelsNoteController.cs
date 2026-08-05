using UnityEngine;
using UnityEngine.UI;

public class KaelsNoteController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;

    public GameControllerLaudos gameControllerLaudos;

    public Dialogue KaelsNoteDialogue1;

    public bool canInteract = true;

    public string sceneID;


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
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance && canInteract)
            {
                if (interactionCount == 0)
                {
                    if (sceneID == "Laudos4")
                    {
                        dialogueController.StartDialogue(KaelsNoteDialogue1);
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
