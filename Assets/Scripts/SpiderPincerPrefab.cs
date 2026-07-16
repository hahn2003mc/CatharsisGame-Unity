using UnityEngine;

public class SpiderPincerPrefab : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float lifetime = 1f;

    [Header("Combat")]
    public float damage = 60f;

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
            Debug.Log("pincer hit player");
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !player.invincible)
            {
                player.health = player.health - damage;
            }

            Destroy(gameObject); // remove after hit
        }
    }

    private void RotateToDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

