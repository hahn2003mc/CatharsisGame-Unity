using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningBugMovement : MonoBehaviour
{
    public float speed = 1f;

    public Vector2 worldMin;
    public Vector2 worldMax;

    private Vector2 target;

    public GameControllerGrassWorld GameControllerGrassWorld;

    void Start()
    {
        GameControllerGrassWorld = GameObject.Find("GameController").GetComponent<GameControllerGrassWorld>();
        PickNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target) < 0.1f || GameControllerGrassWorld.puzzleCompleted)
        {
            PickNewTarget();
        }
        checkWithinRangeOfGate();

    }

    void PickNewTarget()
    {
        if (GameControllerGrassWorld.puzzleCompleted == false)
        {
            target = new Vector2(
                Random.Range(worldMin.x, worldMax.x),
                Random.Range(worldMin.y, worldMax.y)
            );
        }
        else 
        {
            target = new Vector2(53.55f, 7.45f);
            gameObject.GetComponentInChildren<Light2D>().intensity = 2f;
        }
    }

    void checkWithinRangeOfGate()
    {
        if (Vector2.Distance(transform.position, new Vector2(53.55f, 7.45f)) < 0.5f)
        {
            transform.position = new Vector2(
                Random.Range(worldMin.x, worldMax.x),
                Random.Range(worldMin.y, worldMax.y)
            );
        }
    }
}