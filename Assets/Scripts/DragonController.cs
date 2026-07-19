using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    public float maxHealth = 1000f;
    public float health;

    public float fireballDamage = 40f;

    public float detectionRange = 6f;

    public GameObject self;

    Rigidbody2D rb;
    SpriteRenderer sr;

    public GameObject healthBarPrefab;

    private Image healthFill;
    private GameObject healthBarInstance;

    public Animator animator;

    public float fireCooldownLowerBound = 3f;
    public float fireCooldownUpperBound = 5f;

    public float fireballCooldownLowerBound = 1f;
    public float fireballCooldownUpperBound = 2f;

    public float gruntSpawnCooldownLowerBound = 5f;
    public float gruntSpawnCooldownUpperBound = 5f;

    private bool canAttack = true;

    public GameObject fireballPrefab;
    public Transform firePoint;

    public GameObject fireBreathCollider;

    public GameObject gruntPrefab;

    private bool isGracePeriod = true;
    public float gracePeriodHealthThreshold = 0.6f; // 60% health

    public GameController gameController;

    [SerializeField] private string enemyEnum;
    public string EnemyEnum => enemyEnum;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        self.SetActive(true);

        health = maxHealth;

        healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBarInstance.transform.localPosition = new Vector3(0, 4f, 0);

        healthFill = healthBarInstance.transform.Find("EnemyHealthBarHealth").GetComponent<Image>();

        isGracePeriod = true;

        StartCoroutine(RandomFireballAttack());

        fireBreathCollider.SetActive(false);
    }

    void Update()
    {
        // Update health bar
        float percent = health / maxHealth;
        healthFill.fillAmount = percent;

        // once the dragon drops below 60% health, it will start spawning grunts
        if (percent <= gracePeriodHealthThreshold && isGracePeriod)
        {
            isGracePeriod = false;
            StartCoroutine(RandomGruntAttack());
        }

        // Attack logic
        if (canAttack)
        {
            StartCoroutine(RandomFireAttack());
        }
    }

    private IEnumerator RandomFireAttack()
    {
        // Debug.Log("in random fire attack");
        canAttack = false;

        // Wait a random cooldown
        float waitTime = Random.Range(fireCooldownLowerBound, fireCooldownUpperBound);
        yield return new WaitForSeconds(waitTime);
        // Debug.Log("about to call attack");
        FireAttack();

        // Allow next attack
        canAttack = true;
    }

    private IEnumerator RandomFireballAttack()
    {
        // Debug.Log("in random fireball attack");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(fireballCooldownLowerBound, fireballCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("about to call attack");
            FireballAttack();

        }
    }

    private IEnumerator RandomGruntAttack()
    {
        // Debug.Log("in random fireball attack");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(gruntSpawnCooldownLowerBound, gruntSpawnCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("about to call attack");
            SpawnGrunt();

        }
    }

    public void FireAttack()
    {
        // Debug.Log("Dragon attacked!");
        animator.SetTrigger("Attack");
    }

    public void FireballAttack()
    {
        // spawn spell sprite
        Vector3 randomPosition = new Vector3(firePoint.position.x, firePoint.position.y + Random.Range(-2f, 4f), firePoint.position.z);
        GameObject fireballProjectile = Instantiate(fireballPrefab, randomPosition, firePoint.rotation);

        DragonFireball p = fireballProjectile.GetComponent<DragonFireball>();

        p.Initialize(fireballDamage, new Vector2(-1, 0));
    }

    void SpawnGrunt()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x - 1f, transform.position.y + 3f, transform.position.z);
        Instantiate(gruntPrefab, spawnPosition, Quaternion.identity);

        Vector3 spawnPosition2 = new Vector3(transform.position.x - 1f, transform.position.y - 1f, transform.position.z);
        Instantiate(gruntPrefab, spawnPosition2, Quaternion.identity);
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        float percent = health / maxHealth;
        healthFill.fillAmount = percent;

        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Debug.Log("Dragon died");
        gameController.StartSkeletonSpawning(); // stop skeleton spawning when dragon dies
        //animator.SetTrigger("Death");
        gameController.ConfigureEnemyCountsToUpdateAPI(enemyEnum);
        self.SetActive(false);
    }

    public void EnableFireBreath()
    {
        fireBreathCollider.SetActive(true);
    }

    public void DisableFireBreath()
    {
        fireBreathCollider.SetActive(false);
    }
}