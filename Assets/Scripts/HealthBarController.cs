using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public PlayerController playerController;
    public Image healthFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float percent = playerController.health / playerController.maxHealth;
        healthFill.fillAmount = percent;
    }
}
