using UnityEngine;

public class KnightSwordController : MonoBehaviour
{
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
