using System.Collections;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    [SerializeField] GameObject self;
    private int direction;
    public float flipSpeed;
    public float moveSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (self == null) 
        {
            self = gameObject;
        }
        if (flipSpeed == 0)
        {
            flipSpeed = 3.0f;
        }
        if (moveSpeed == 0)
        {
            moveSpeed = 2.5f;
        }
        direction = -1;
        StartCoroutine(waitAndFlip());
    }

    void Update() 
    {
        if (direction == -1) 
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }
        else if (direction == 1)
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }
    }

    private IEnumerator waitAndFlip() {
        yield return new WaitForSeconds(flipSpeed);
        if (direction == -1)
        {
            direction = 1;
        }
        else { 
            direction = -1; 
        }
        StartCoroutine(waitAndFlip());
    }
}
