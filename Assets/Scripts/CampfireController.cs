using UnityEngine;

public class CampfireController : MonoBehaviour
{
    public GameObject player;
    public GameObject self;

    private PlayerController playerController;

    public float interactionDistance = 2f;

    public float healingAmount = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(player.transform.position, self.transform.position) < interactionDistance && (playerController.health < 100))
        {
            Debug.Log("within campfire range and not max health");
            playerController.health += healingAmount * Time.deltaTime;
        }
        
    }
}
