using UnityEngine;
using UnityEngine.UI;

public class DadController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public DialogueController dialogueController;
    public Dialogue CatharinAndDadDialogue1;
    public Dialogue CatharinAndDadDialogue2;
    public Dialogue CatharinAndDadDialogue3;

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
                if (interactionCount == 0)
                {
                    dialogueController.StartDialogue(CatharinAndDadDialogue1);
                }
                else if (interactionCount == 1)
                {
                    dialogueController.StartDialogue(CatharinAndDadDialogue2);
                }
                else if (interactionCount == 2)
                {
                    dialogueController.StartDialogue(CatharinAndDadDialogue3);
                }
            }
        }
    }

    public void updateInteractionCount(int value)
    {
        interactionCount = value;
    }

}
