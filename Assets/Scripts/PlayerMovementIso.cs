using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovementIso : MonoBehaviour
{

    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    public Camera mainCamera;

    private SpriteRenderer sr;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        HandleMovement();
        {
            GetComponent<SpriteRenderer>().sortingOrder =
                Mathf.RoundToInt(transform.position.y * -1);
        }
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -1000);
    }
    
    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        // Flip across X axis when D is pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Ensure positive
            transform.localScale = scale;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 scale = transform.localScale;   
            scale.x = -Mathf.Abs(scale.x); // Ensure negative
            transform.localScale = scale;
        }

    }

}
