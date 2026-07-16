using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MotherSpiderController : MonoBehaviour
{
    public float maxHealth = 1000f;
    public float health;

    public float pincerDamage = 40f;

    public float detectionRange = 6f;

    public GameObject self;

    Rigidbody2D rb;
    SpriteRenderer sr;

    public GameObject healthBarPrefab;

    private Image healthFill;
    private GameObject healthBarInstance;

    public Animator animator;

    public float stompCooldownLowerBound = 3f;
    public float stompCooldownUpperBound = 5f;

    public float pincerCooldownLowerBound = 10f;
    public float pincerCooldownUpperBound = 20f;

    public float gruntSpawnCooldownLowerBound = 8f;
    public float gruntSpawnCooldownUpperBound = 16f;

    public float webSpawnCooldownLowerBound = 8f;
    public float webSpawnCooldownUpperBound = 16f;

    private bool canAttack = true;

    public GameObject pincerPrefab;
    public Transform firePoint;

    public GameObject stompCollider;

    public GameObject gruntPrefab;
    public GameObject webPrefab;
    public int webSpawnCount = 5;

    public GameControllerGrassWorld gameControllerGrassWorld;

    public PlayerController player;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        health = maxHealth;

        healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBarInstance.transform.localPosition = new Vector3(0, 1f, 0);

        healthFill = healthBarInstance.transform.Find("EnemyHealthBarHealth").GetComponent<Image>();

        StartCoroutine(RandomPincerAttack());
        // StartCoroutine(RandomGruntAttack());
        StartCoroutine(RandomWebAttack());

        stompCollider.SetActive(false);
    }

    void Update()
    {
        // Update health bar
        float percent = health / maxHealth;
        healthFill.fillAmount = percent;
        

        // Attack logic
        if (canAttack)
        {
            StartCoroutine(RandomStompAttack());
        }
    }

    private IEnumerator RandomStompAttack()
    {
        //Debug.Log("in random stomp attack");
        canAttack = false;

        // Wait a random cooldown
        float waitTime = Random.Range(stompCooldownLowerBound, stompCooldownUpperBound);
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("about to call attack");
        StompAttack();

        // Allow next attack
        canAttack = true;
    }

    private IEnumerator RandomPincerAttack()
    {
        Debug.Log("in random pincer attack");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(pincerCooldownLowerBound, pincerCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            Debug.Log("about to call pincer attack");
            PincerAttack();

        }
    }

    private IEnumerator RandomGruntAttack()
    {
        Debug.Log("in random grunt attack");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(gruntSpawnCooldownLowerBound, gruntSpawnCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("about to call attack");
            SpawnGrunt();

        }
    }

    private IEnumerator RandomWebAttack()
    {
        Debug.Log("in random web attack");
        while (true)
        {

            // Wait a random cooldown
            float waitTime = Random.Range(webSpawnCooldownLowerBound, webSpawnCooldownUpperBound);
            yield return new WaitForSeconds(waitTime);
            //Debug.Log("about to call attack");
            SpawnWebs();

        }
    }

    public void StompAttack()
    {
        //Debug.Log("Spider attacked!");
        animator.SetTrigger("Attack");
    }

    public void PincerAttack()
    {
        Debug.Log("in pincer attack function");
        // spawn pincer sprite
        Vector3 spawnPosition = new Vector3(
            firePoint.position.x,
            firePoint.position.y,
            firePoint.position.z
        );
        GameObject pincerProjectile = Instantiate(pincerPrefab, spawnPosition, firePoint.rotation);

        SpiderPincerPrefab p = pincerProjectile.GetComponent<SpiderPincerPrefab>();

        Vector2 targetDirection = (player.transform.position + new Vector3(0, 2, 0) - spawnPosition).normalized;

        p.Initialize(pincerDamage, targetDirection);
        Debug.Log("pincer attack should be launched");
    }

    void SpawnGrunt()
    {
        // set random position here
        Vector3 spawnPosition = new Vector3(transform.position.x - 1f, transform.position.y + 3f, transform.position.z);
        Instantiate(gruntPrefab, spawnPosition, Quaternion.identity);

        Vector3 spawnPosition2 = new Vector3(transform.position.x - 1f, transform.position.y - 1f, transform.position.z);
        Instantiate(gruntPrefab, spawnPosition2, Quaternion.identity);

    }

    void SpawnWebs()
    {
        int i = 0;
        while (i < webSpawnCount)
        {
            // set random position here
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-16f , 4f), transform.position.y + Random.Range(-12f, 12f), transform.position.z);
            Instantiate(webPrefab, spawnPosition, Quaternion.identity);

            i++;
        }

    }

    // spawn web here

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
        Debug.Log("Mother spider died");
        gameControllerGrassWorld.motherSpiderDefeated();
        //animator.SetTrigger("Death");
        self.SetActive(false);
    }

    public void EnableStomp()
    {
        stompCollider.SetActive(true);
    }

    public void DisableStomp()
    {
        stompCollider.SetActive(false);
    }
}