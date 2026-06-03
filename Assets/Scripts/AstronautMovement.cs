using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class AstronautMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    public Camera mainCamera;

    private SpriteRenderer sr;

    public bool isWalking = false;

    private Vector2 lastMove = Vector2.down;

    private Animator animator;

    private string lastDirection = "IdleDown";

    public float health = 100f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        isWalking = ((Input.GetAxisRaw("Horizontal") != 0) || ((Input.GetAxisRaw("Vertical") != 0)));
        
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
            lastDirection = "IdleLeft";
}
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // Ensure negative
            transform.localScale = scale;
            lastDirection = "IdleRight";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            lastDirection = "IdleUp";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirection = "IdleDown";
        }

        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);

        animator.SetBool("isWalking", isWalking);

        if (!isWalking)
        {
            animator.Play(lastDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if object is on Weapon layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // Try to get Weapon component
            Weapon weapon = other.GetComponent<Weapon>();

            if (weapon != null)
            {
                HandleDamage(weapon.damage);
            }
        }
    }

    void HandleDamage(float incomingDamage)
    {
        health -= incomingDamage;
        Debug.Log("Player took " + incomingDamage + " damage. Current health: " + health);

        if (health <= 0)
        {
            // Handle player death (e.g., play animation, disable controls, etc.)
            Debug.Log("Player has died!");
        }
    }
}
