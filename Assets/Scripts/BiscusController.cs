using UnityEngine;
using UnityEngine.UI;

public class BiscusController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;
    public Dialogue CatharinAndBiscusDialogue1;

    public GameControllerLaudos GameControllerLaudos;

    public GameObject biscusInteractionCollider;

    public bool canInteract = true;



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
                    biscusInteractionCollider.SetActive(false);
                    dialogueController.StartDialogue(CatharinAndBiscusDialogue1);
                }
                else if (interactionCount == 1)
                {
                    // do nothing
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
