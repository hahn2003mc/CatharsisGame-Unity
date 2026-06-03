using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GruntController : MonoBehaviour
{
    public float maxHealth = 50f;
    public float movementSpeed = 8f;
    public float health;

    public float detectionRange = 15f;

    public float attackRange = 0.2f;

    public GameObject target;
    public GameObject self;

    Rigidbody2D rb;

    SpriteRenderer sr;

    public Weapon weapon;

    public GameObject healthBarPrefab;

    private Image healthFill;
    private GameObject healthBarInstance;

    public Animator animator;

    public AStarPathFinder pathFinder;
    public PathFindingGrid pathFindingGrid;

    public GameObject colliderTilemapObject;
    public Tilemap colliderTilemap;

    private List<Node> path;
    private int currentNodeIndex = 0;

    public float pathUpdateRate = 0.5f;
    private Vector3 lastTargetPos;

    private bool canAttack = true;
    private bool isBackingOff = false;

    public float despawnDistance = 35f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        self.SetActive(true);

        health = maxHealth;

        healthBarInstance = Instantiate(healthBarPrefab, transform);
        healthBarInstance.transform.localPosition = new Vector3(0, 1.2f, 0);

        healthFill = healthBarInstance.transform.Find("EnemyHealthBarHealth").GetComponent<Image>();

        target = GameObject.Find("Player");

        colliderTilemapObject = GameObject.Find("ColliderTilemap");
        colliderTilemap = colliderTilemapObject.GetComponent<Tilemap>();

        pathFindingGrid = GameObject.Find("Navigation").GetComponent<PathFindingGrid>();
        pathFinder = GameObject.Find("Navigation").GetComponent<AStarPathFinder>();

        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        float percent = health / maxHealth;
        healthFill.fillAmount = percent;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        //Debug.Log("Distance from player is " + distance);

        if (distance > despawnDistance)
        {
            Destroy(gameObject); // simple, safe
        }

        if (health <= 0)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (target != null)
            {
                if (Vector3.Distance(target.transform.position, lastTargetPos) > 0.5f)
                {
                    path = pathFinder.FindPath(transform.position, target.transform.position);

                    currentNodeIndex = 0;
                    lastTargetPos = target.transform.position;
                }
            }

            yield return new WaitForSeconds(pathUpdateRate);
        }
    }

    void FollowTarget()
    {
        if (path == null || path.Count == 0)
            return;

        Vector2 targetPosition;

        if (currentNodeIndex >= path.Count)
            targetPosition = target.transform.position;
        else
            targetPosition = path[currentNodeIndex].worldPosition;

        // Move toward target
        if (canAttack && Vector2.Distance(transform.position, target.transform.position) <= detectionRange && health > 0)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, movementSpeed * Time.fixedDeltaTime));
        }

        // check if reached node
        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            currentNodeIndex++;
        }

        // Check if reached target
        if (!isBackingOff && Vector2.Distance(transform.position, target.transform.position) <= attackRange + 0.1f)
        {
            StartCoroutine(BackOffAndAttack());
        }

        // Flip sprite toward player
        float xDifference = target.transform.position.x - transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(target.transform.position.x - transform.position.x);
        transform.localScale = scale;
    }

    IEnumerator BackOffAndAttack()
    {
        isBackingOff = true;
        canAttack = false;

        Vector2 directionAway = (transform.position - target.transform.position).normalized;
        Vector2 backPosition = (Vector2)transform.position + directionAway * 0.5f; // distance to step back

        float timer = 0;
        float duration = 0.2f;

        while (timer < duration)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, backPosition, movementSpeed * Time.fixedDeltaTime));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Trigger attack animation
        //animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f); // attack windup

        canAttack = true;
        isBackingOff = false;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("took damage");
        health = health - damage;

        float percent = health / maxHealth;
        healthFill.fillAmount = percent;

        if (health <= 0)
        {
            weapon.gameObject.SetActive(false);
            Destroy(weapon.gameObject);
            animator.SetTrigger("Death");
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Death()
    {
        rb.linearVelocity = Vector2.zero;
        self.SetActive(false);
        Destroy(gameObject);
    }
}