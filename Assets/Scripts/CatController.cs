using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
    public GameObject TextUIWizard;
    public GameObject TextUIKnight;
    //public GameObject InteractMarker;

    public GameObject Text0Wizard;
    public GameObject Text0Knight;

    public GameObject Player;
    public GameObject self;

    public GameObject Knight;
    public GameObject Wizard;

    public float interactionDistance = 1f;

    public int interactionCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextUIWizard.SetActive(false);
        TextUIKnight.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TextUIWizard.SetActive(false);
            TextUIKnight.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance)
            {
                Debug.Log("pressed F");
                interactionCount = 0;
                if (Knight.activeInHierarchy)
                {
                    TextBoxControllerKnight();
                }
                else if (Wizard.activeInHierarchy)
                {
                    TextBoxControllerWizard();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && interactionCount > 0)
        {
            if (Vector3.Distance(Player.transform.position, self.transform.position) < interactionDistance)
            {
                Debug.Log("pressed space");
                if (Knight.activeInHierarchy)
                {
                    TextBoxControllerKnight();
                }
                else if (Wizard.activeInHierarchy)
                {
                    TextBoxControllerWizard();
                }
            }
        }
    }

    void TextBoxControllerKnight()
    {
        if (interactionCount == 0) 
        {
            TextUIWizard.SetActive(false);
            TextUIKnight.SetActive(true);
            Text0Knight.SetActive(true);
            interactionCount++;
        }
        else if (interactionCount == 1)
        {
            TextUIWizard.SetActive(false);
            TextUIKnight.SetActive(false);
            Text0Knight.SetActive(false);
            Text0Wizard.SetActive(false);
            interactionCount = 0;
        }
    }
    void TextBoxControllerWizard()
    {
        Debug.Log("in wizard text box controller");
        if (interactionCount == 0)
        {
            Debug.Log("in wizard text box controller interaction count 0");
            TextUIKnight.SetActive(false);
            TextUIWizard.SetActive(true);
            Text0Wizard.SetActive(true);
            interactionCount++;
        }
        else if (interactionCount == 1)
        {
            Debug.Log("in wizard text box controller interaction count 1");
            TextUIWizard.SetActive(false);
            TextUIKnight.SetActive(false);
            Text0Wizard.SetActive(false);
            Text0Knight.SetActive(false);
            interactionCount = 0;
        }
    }
}
