using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float lifetime = 2f;

    [Header("Combat")]
    public float damage = 40f;

    private Vector2 direction;

    public void Initialize(float dmg, Vector2 dir)
    {
        damage = dmg;
        direction = dir.normalized;

        RotateToDirection();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !player.invincible)
            {
                player.health = player.health - damage;
            }

            Destroy(gameObject); // remove after hit (optional)
        }
    }

    private void RotateToDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

