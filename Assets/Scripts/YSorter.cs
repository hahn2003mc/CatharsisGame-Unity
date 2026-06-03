using UnityEngine;

public class YSorter : MonoBehaviour
{
    private SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
