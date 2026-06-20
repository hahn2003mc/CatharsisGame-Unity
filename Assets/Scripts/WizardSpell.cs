using UnityEngine;

public class WizardSpell : MonoBehaviour
{
    public WizardController wizardController;

    public float speed;
    public float lifetime;

    public float damage;

    private Vector2 direction;

    public void Initialize(float dmg, Vector2 dir, float speed, float lifetime)
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
        if (other.CompareTag("Enemy"))
        {
            GruntController enemy = other.GetComponent<GruntController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // remove after hit (optional)
        }
        if (other.CompareTag("Dragon"))
        {
            DragonController enemy = other.GetComponent<DragonController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // remove after hit (optional)
        }
        if (other.CompareTag("MotherSpider"))
        {
            MotherSpiderController enemy = other.GetComponent<MotherSpiderController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
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