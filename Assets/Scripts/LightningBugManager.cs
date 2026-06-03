using UnityEngine;

public class LightningBugSpawner : MonoBehaviour
{
    public GameObject lightningBugPrefab;
    public int bugCount = 15;

    public Vector2 worldMin;
    public Vector2 worldMax;

    void Start()
    {
        for (int i = 0; i < bugCount; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(worldMin.x, worldMax.x),
                Random.Range(worldMin.y, worldMax.y)
            );

            GameObject bug = Instantiate(lightningBugPrefab, pos, Quaternion.identity);

            bug.GetComponent<LightningBugMovement>().worldMin = worldMin;
            bug.GetComponent<LightningBugMovement>().worldMax = worldMax;
        }
    }
}