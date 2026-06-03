using UnityEngine;

public class DragonFireBreath : MonoBehaviour
{
    [Header("Combat")]
    public float damage = 100f;

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
}

