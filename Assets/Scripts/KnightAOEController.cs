using UnityEngine;

public class KnightAOEController : MonoBehaviour
{
    public GameObject self;
    public PlayerController playerController;
    public KnightController knightController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        knightController.playerController.GetComponent<KnightController>();
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
        playerController.ResetPlayerValues();
        self.SetActive(false);
        knightController.canSummonAOE = true;
    }
}
