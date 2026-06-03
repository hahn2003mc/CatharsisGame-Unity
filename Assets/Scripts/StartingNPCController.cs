using UnityEngine;
using UnityEngine.UI;

public class StartingNPCController : MonoBehaviour
{
    public GameObject TextUI;
    public GameObject InteractMarker;

    public GameObject Text0;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;
    public GameObject Text4;

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextUI.SetActive(false);
        InteractMarker.SetActive(true);

        interactionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("within collider and pressed interact button (Esc)");

            interactionCount = 0;

            TextUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
            {
                interactionCount = 0;
                Debug.Log("within collider and pressed interact button (F)");
                TextBoxController();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && interactionCount > 0)
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
            {
                Debug.Log("within collider and pressed interact button (F)");
                TextBoxController();
            }
        }
    }

    void TextBoxController()
    {
        // first interact
        if (interactionCount == 0)
        {
            TextUI.SetActive(true);
            Text0.SetActive(true);
            Text1.SetActive(false);
            Text2.SetActive(false);
            Text3.SetActive(false);
            Text4.SetActive(false);
            interactionCount++;
        }
        else if (interactionCount == 1)
        {
            TextUI.SetActive(true);
            Text0.SetActive(false);
            Text1.SetActive(true);
            Text2.SetActive(false);
            Text3.SetActive(false);
            Text4.SetActive(false);
            interactionCount++;
        }
        else if (interactionCount == 2)
        {
            TextUI.SetActive(true);
            Text0.SetActive(false);
            Text1.SetActive(false);
            Text2.SetActive(true);
            Text3.SetActive(false);
            Text4.SetActive(false);
            interactionCount++;
        }
        else if (interactionCount == 3)
        {
            TextUI.SetActive(true);
            Text0.SetActive(false);
            Text1.SetActive(false);
            Text2.SetActive(false);
            Text3.SetActive(true);
            Text4.SetActive(false);
            interactionCount++;
        }
        else if (interactionCount == 4)
        {
            TextUI.SetActive(true);
            Text0.SetActive(false);
            Text1.SetActive(false);
            Text2.SetActive(false);
            Text3.SetActive(false);
            Text4.SetActive(true);
            interactionCount++;
        }

        else if (interactionCount > 4)
        {
            Text0.SetActive(false);
            Text1.SetActive(false);
            Text2.SetActive(false);
            Text3.SetActive(false);
            Text4.SetActive(false);
            TextUI.SetActive(false);
            interactionCount = 0;
        }
    }
}
