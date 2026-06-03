using UnityEngine;

public class BrushController : MonoBehaviour
{
    public PlayerController playerController;
    //public GameObject self;
    public float damageAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageAmount = 1f;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(playerController.transform.position, transform.position) < 0.5f)
        {
            //Debug.Log("within campfire range and not max health");
            playerController.health -= damageAmount * Time.deltaTime;
        }

    }
}
