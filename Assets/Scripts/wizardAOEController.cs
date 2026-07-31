using UnityEngine;

public class WizardAOEController : MonoBehaviour
{
    public GameObject self;
    public PlayerController playerController;
    public WizardController wizardController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wizardController.playerController.GetComponent<WizardController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selfSetActiveTrue()
    {
        self.SetActive(true);
    }

    public void selfSetActiveFalse()
    {
        playerController.ResetPlayerSpeeds();
        self.SetActive(false);
        wizardController.canSummonAOE = true;
    }
}
