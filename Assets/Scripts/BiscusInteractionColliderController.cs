using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BiscusInteractionColliderController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject self;

    public DialogueController dialogueController;
    public Dialogue CatharinAndBiscusDialogue1;


    void Start()
    {
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("something entered the collider");
        if (other.CompareTag("Player") ||
    (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("Player or direct child entered");
            dialogueController.StartDialogue(CatharinAndBiscusDialogue1);
            self.SetActive(false);
        }
    }
}
