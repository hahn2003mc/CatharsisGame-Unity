using UnityEngine;

public class MotherSpiderWebController : MonoBehaviour
{
    public PlayerController playerController;
    //public GameObject self;
    public float damageAmount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("WEB DISTANCE: " + Vector3.Distance(playerController.transform.position, transform.position));
        if (Vector3.Distance(playerController.transform.position, transform.position) < 1f)
        {
            //Debug.Log("within campfire range and not max health");
            playerController.health -= damageAmount * Time.deltaTime;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other + " entered the web collider");
        KnightController knight = other.GetComponent<KnightController>();
        if (knight != null)
        {
            //Debug.Log("KNIGHT entered the web collider");
            knight.SetSpeedMultiplier(0.5f);
        }

        WizardController wizard = other.GetComponent<WizardController>();
        if (wizard != null)
        {
            wizard.SetSpeedMultiplier(0.5f);
        }

        WizardSpell wizardSpell = other.GetComponent<WizardSpell>();
        if (wizardSpell != null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        KnightController knight = other.GetComponent<KnightController>();
        if (knight != null)
        {
            knight.ResetSpeedMultiplier();
        }

        WizardController wizard = other.GetComponent<WizardController>();
        if (wizard != null)
        {
            wizard.ResetSpeedMultiplier();
        }
    }


}
