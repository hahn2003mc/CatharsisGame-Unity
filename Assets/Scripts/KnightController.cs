using UnityEngine;
using UnityEngine.Rendering.Universal;
using static KnightFormController;

public class KnightController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    public PlayerController playerController;
    KnightFormController form;

    public float moveSpeed = 3f;

    public bool isWalking = false;

    public Camera mainCamera;

    public float swordDamage = 30f;
    public float heavyAttackDamage = 90f;
    public float slashAttackDamage = 150f;

    public float maxEnergy = 100f;

    public float attackEnergyCost = 50f;
    public float heavyAttackEnergyCost = 100f;
    public float slashAttackEnergyCost = 100f;

    public float energyRegenRate = 10f;

    public float currentEnergy;

    public Transform firePoint;

    public float moveX;

    public float moveY;

    public GameObject swordObject;
    public GameObject swordAttackCollider;
    public GameObject heavyAttackCollider;
    public GameObject slashAttackCollider;

    public bool canMove;
    [SerializeField] private bool canAttack;

    public Light2D knightSwordLight;

    public bool lockCameraToPlayer = true;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        form = GetComponent<KnightFormController>();

        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentEnergy = maxEnergy;

        //swordObject.SetActive(true);
        swordAttackCollider.SetActive(false);
        heavyAttackCollider.SetActive(false);

        // canMove = true;
        playerController.invincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockCameraToPlayer)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }

        if (canMove)
        {
            HandleMovement();
        }
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        if (canAttack)
        {
            HandleAttack();
        }

        RegenerateEnergy();

        EnergyDisplay();

        HandleSwap();
    }

    void HandleMovement()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX, moveY) * baseMoveSpeed * speedMultiplier;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);


        // Flip across X axis when D is pressed
        if (Input.GetKey(Bindings.KnightMoveLeft))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // Ensure positive
            transform.localScale = scale;

            playerController.lastDirection = Vector2.left;
        }
        else if (Input.GetKey(Bindings.KnightMoveRight))
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Ensure negative
            transform.localScale = scale;

            playerController.lastDirection = Vector2.right;
        }
        else if (Input.GetKey(Bindings.KnightMoveUp))
        {
            playerController.lastDirection = Vector2.up;
        }
        else if (Input.GetKey(Bindings.KnightMoveDown))
        {
            playerController.lastDirection = Vector2.down;
        }

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isWalking = moveInput.sqrMagnitude > 0;

        if (isWalking)
        {
            playerController.lastDirection = moveInput.normalized;
        }

        animator.SetBool("isWalking", isWalking);

        float blendX = isWalking ? moveInput.x : playerController.lastDirection.x;
        float blendY = isWalking ? moveInput.y : playerController.lastDirection.y;

        animator.SetFloat("moveX", blendX);
        animator.SetFloat("moveY", blendY);
    }

    void HandleSwap()
    {
        if (Input.GetKeyDown(Bindings.KnightSwapForm))
        {
            if (form.currentForm == KnightFormController.KnightForm.Armor)
                form.SetForm(KnightFormController.KnightForm.Girl);
            else
                form.SetForm(KnightFormController.KnightForm.Armor);
        }
        if (Input.GetKeyDown(Bindings.KnightSwapSword))
        {
            if (swordObject.activeSelf)
            {
                swordObject.SetActive(false);
            }
            else
            {
                swordObject.SetActive(true);
            }
        }
    }

    void HandleAttack()
    {
        // on mouse pressed
        if (Input.GetMouseButtonDown(Bindings.KnightLightAttack))
        {
            //Debug.Log("left mouse clicked on knight");
            if (currentEnergy >= attackEnergyCost)
            {
                // start attacking animation
                animator.SetTrigger("Attack");

                // subtract mana
                currentEnergy = currentEnergy - attackEnergyCost;
            }
            else
            {
                Debug.Log("no energy!");
            }
        }
        // on Q pressed
        if (Input.GetKeyDown(Bindings.KnightHeavyAttack))
        {
            //Debug.Log("Q pressed on knight");
            if (currentEnergy >= heavyAttackEnergyCost)
            {
                // start attacking animation
                animator.SetTrigger("HeavyAttack");
                // subtract mana
                currentEnergy = currentEnergy - heavyAttackEnergyCost;
            }
            else
            {
                Debug.Log("no energy!");
            }
        }
        // on E pressed in Girl Form
        if (Input.GetKeyDown(Bindings.KnightSlashAttack) && form.currentForm == KnightFormController.KnightForm.Girl)
        {
            //Debug.Log("E pressed on knight in girl form");
            if (currentEnergy >= slashAttackEnergyCost)
            {
                // start attacking animation
                animator.SetTrigger("SlashAttack");
                // subtract mana
                currentEnergy = currentEnergy - slashAttackEnergyCost;
            }
            else
            {
                Debug.Log("no energy!");
            }
        }
    }

    void OnEnable()
    {
        canMove = true;
        ApplyFacingDirection();
    }

    public void ApplyFacingDirection()
    {
        if (playerController == null) { return; }
        Vector2 dir = playerController.lastDirection;
        if (dir == null) { return; }

        Vector3 scale = transform.localScale;

        if (dir.x < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (dir.x > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;

        if (animator == null) { return; }
        animator.SetFloat("moveX", dir.x);
        animator.SetFloat("moveY", dir.y);
    }

    void RegenerateEnergy()
    {
        /*
        // if current energy less than max energy
        if (currentEnergy < maxEnergy)
        {
            // regenerate energy
            currentEnergy += energyRegenRate * Time.deltaTime;
            // clamp to max energy
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
            }
        }
        */
    }

    private float baseMoveSpeed = 3f;
    private float speedMultiplier = 1f;

    public void SetSpeedMultiplier(float multiplier)
    {
        Debug.Log("setting speed multiplier to " + multiplier);
        speedMultiplier = multiplier;
    }

    public void ResetSpeedMultiplier()
    {
        Debug.Log("resetting speed multiplier to " + 1);
        speedMultiplier = 1f;
    }

    void EnergyDisplay()
    {
        float percentage = currentEnergy / maxEnergy;
        knightSwordLight.intensity = percentage * 3;
    }

    public void EnableSword()
    {
        swordObject.SetActive(true);
    }

    public void EnableSwordCollider()
    {
       swordAttackCollider.SetActive(true);
    }

    public void DisableSword()
    {
        swordObject.SetActive(false);
    }

    public void DisableSwordCollider()
    {
        swordAttackCollider.SetActive(false);

    }

    public void EnableHeavyAttackCollider()
    {
        heavyAttackCollider.SetActive(true);
    }

    public void DisableHeavyAttackCollider()
    {
        heavyAttackCollider.SetActive(false);

    }

    public void EnableSlashAttackCollider()
    {
        slashAttackCollider.SetActive(true);
    }

    public void DisableSlashAttackCollider()
    {
        slashAttackCollider.SetActive(false);

    }

    public void setCanMoveTrue()
    {
        canMove = true;
    }

    public void setInvincibleTrue()
    {
        playerController.invincible = true;
    }

    public void setInvincibleFalse()
    {
        playerController.invincible = false;
    }

    public void setCanMoveFalse()
    {
        canMove = false;
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody2D>();
        }
        rb.linearVelocity = Vector2.zero;
    }

    public void setCanAttack(bool var) 
    {
        canAttack = var;
    }

    public void setCanAttackTrue() 
    {
        canAttack = true;
    }

    public void setCanAttackFalse()
    {
        canAttack = false;
    }

    public void setCanSwapCharactersTrueInParent() 
    {
        playerController.setCanSwapCharactersTrue();
    }

    public void setCanSwapCharactersFalseInParent()
    {
        playerController.setCanSwapCharactersFalse();
    }
}
