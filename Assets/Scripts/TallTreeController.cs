using UnityEngine;

public class TallTreeController : MonoBehaviour
{
    public GameObject player;
    public GameObject self;
    public float interactionDistance = 1f;

    public string treeNumber;

    public GameControllerGrassWorld gameController;

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
                gameController.TreeTracker(treeNumber);
            }
        }
    }
}
