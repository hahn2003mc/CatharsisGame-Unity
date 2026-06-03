using UnityEngine;

public class FlowerController : MonoBehaviour
{
    public GameObject player;
    public GameObject self;
    public float interactionDistance = 1f;

    public int flowerNumber;

    public GameController gameController;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(player.transform.position, self.transform.position) < interactionDistance)
            {
                Debug.Log("within collider and pressed interact button (F)");
                gameController.FlowerTracker(flowerNumber);
            }
        }
    }
}
