using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WizardController : MonoBehaviour
{
    public PlayerController playerController;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    public float moveSpeed = 3f;

    public bool isWalking = false;

    public Camera mainCamera;

    public float maxMana;

    public float spellManaCost;

    public float healManaCost;

    public float heavyAttackCost;

    public GameObject WizardAOE;
    public float buffCost;
    public float buffSpeedMultiplier;
    public bool canSummonAOE;
    public int AOESpellCount;

    public float manaRegenRate;

    public float currentMana;

    public GameObject spellPrefab;
    public GameObject heavySpellPrefab;

    public Transform firePoint;
    public Transform bookFirePoint;

    public float moveX;

    public float moveY;

    public GameObject wizardBook;
    public Light2D WizardBookLight;

    public GameObject healAnimationObject;
    
    public float healingAmount;

    public GameObject healingMarker;

    public bool canMove;

    public float spellDamage;
    public float spellSpeed;
    public float spellLifetime;
    public float heavySpellDamage;
    public float heavySpellSpeed;
    public float heavySpellLifetime;


    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        rb = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentMana = maxMana;

        healAnimationObject.SetActive(false);
        healingMarker.SetActive(false);

        maxMana = PlayerValues.WizardMaxMana;
        spellManaCost = PlayerValues.WizardLightAttackManaCost;
        healManaCost = PlayerValues.WizardHealManaCost;
        manaRegenRate = PlayerValues.WizardManaRegenRate;
        healingAmount = PlayerValues.WizardHealHealingAmount;
        spellDamage = PlayerValues.WizardLightAttackDamage;
        spellSpeed = PlayerValues.WizardLightAttackSpeed;
        spellLifetime = PlayerValues.WizardLightAttackLifetime;
        heavyAttackCost = PlayerValues.WizardHeavyAttackCost;
        heavySpellDamage = PlayerValues.WizardHeavyAttackDamage;
        heavySpellSpeed = PlayerValues.WizardHeavyAttackSpeed;
        heavySpellLifetime = PlayerValues.WizardHeavyAttackLifetime;
        buffCost = PlayerValues.WizardAOECost;
        buffSpeedMultiplier = PlayerValues.WizardAOESpeedMultiplier;
        moveSpeed = PlayerValues.WizardMoveSpeed;
        WizardAOE.SetActive(false);
        AOESpellCount = PlayerValues.WizardAOESpellCount;

        bookFirePoint = wizardBook.GetComponent<Transform>();

        // canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        if (canMove)
        {
            HandleMovement();
        }
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        HandleAttack();

        ManaDisplay();

    }

    void HandleMovement()
    {
        isWalking = ((Input.GetAxisRaw("Horizontal") != 0) || ((Input.GetAxisRaw("Vertical") != 0)));

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(moveX, moveY) * moveSpeed;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);


        // Flip across X axis when D is pressed
        if (Input.GetKeyDown(Bindings.WizardMoveLeft))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // Ensure positive
            transform.localScale = scale;

            playerController.lastDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(Bindings.WizardMoveRight))
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // Ensure negative
            transform.localScale = scale;

            playerController.lastDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(Bindings.WizardMoveUp))
        {

            playerController.lastDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(Bindings.WizardMoveDown))
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

    void HandleAttack()
    {
        // on mouse pressed
        if (Input.GetMouseButtonDown(Bindings.WizardLightAttack))
        {
            // if enough mana, cast spell
            if (currentMana >= spellManaCost)
            {
                // start attacking animation
                animator.Play("WizardStillRightSpell");

                animator.SetTrigger("CastSpell");

                currentMana -= spellManaCost;

                Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                Vector2 direction = (mouseWorld - firePoint.position).normalized;

                GameObject spellProjectile = Instantiate(spellPrefab, firePoint.position, Quaternion.identity);

                WizardSpell p = spellProjectile.GetComponent<WizardSpell>();
                p.Initialize(spellDamage, direction, spellSpeed, spellLifetime);
            }
            else 
            {
                Debug.Log("not enough mana!");
            }
        }
        // on E pressed
        if (Input.GetKeyDown(Bindings.WizardHeal))
        {
            if (currentMana >= healManaCost)
            {
                currentMana = currentMana - healManaCost;

                healAnimationObject.SetActive(true);
                healAnimationObject.GetComponent<Animator>().Play("HealAnimation");
                healingMarker.SetActive(true);

                Invoke(nameof(DisableHealAnimation), 0.3f);
                Invoke(nameof(DisableHealingMarker), 0.3f);

                playerController.health = Mathf.Min(playerController.health + healingAmount, playerController.maxHealth);
            }
            else
            {
                Debug.Log("not enough mana!");
            }
        }
        if (Input.GetMouseButtonDown(Bindings.WizardHeavyAttack))
        {
            if (currentMana >= heavyAttackCost)
            {
                currentMana = currentMana - heavyAttackCost;

                wizardBook.GetComponent<Animator>().Play("Red");

                Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                Vector2 direction = (mouseWorld - wizardBook.transform.position).normalized;

                GameObject spellProjectile = Instantiate(heavySpellPrefab, wizardBook.transform.position, Quaternion.identity);

                WizardHeavySpell p = spellProjectile.GetComponent<WizardHeavySpell>();
                p.Initialize(heavySpellDamage, direction, heavySpellSpeed, heavySpellLifetime);
            }
            else
            {
                Debug.Log("not enough mana!");
            }
        }
        if (Input.GetKeyDown(Bindings.WizardBuff) && canSummonAOE)
        {
            if (currentMana >= buffCost)
            {
                canSummonAOE = false;

                currentMana = currentMana - buffCost;

                WizardAOE.SetActive(true);
                WizardAOE.transform.position = playerController.transform.position + new Vector3(0, 0.1f, 0);

                playerController.UpdatePlayerSpeedsWithMultiplier();

                Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                Vector2 direction = (mouseWorld - firePoint.transform.position).normalized;


                int i = 0;
                while (i < AOESpellCount) {
                    GameObject spellProjectile = Instantiate(heavySpellPrefab, firePoint.transform.position + new Vector3(0, i-2, 0), Quaternion.identity);

                    WizardHeavySpell p = spellProjectile.GetComponent<WizardHeavySpell>();

                    p.Initialize((float)Math.Floor(heavySpellDamage/2), direction, heavySpellSpeed, heavySpellLifetime);
                    i++;
                }
            }
            else
            {
                Debug.Log("not enough mana!");
            }
        }
    }

    void ManaDisplay()
    {
        float percentage = currentMana / maxMana;
        WizardBookLight.intensity = percentage * 5;
    }

    public void ResetSpeed()
    {
        moveSpeed = PlayerValues.WizardMoveSpeed;
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
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (animator != null)
        {
            animator.SetFloat("moveX", dir.x);
            animator.SetFloat("moveY", dir.y);
        }
    }

    public void DisableHealAnimation()
    {
        healAnimationObject.SetActive(false);
    }

    public void EnableHealingMarker()
    {
        healingMarker.SetActive(true);
    }

    public void DisableHealingMarker()
    {
        healingMarker.SetActive(false);
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
        if (rb == null) {
            rb = GetComponentInParent<Rigidbody2D>();
        }
        rb.linearVelocity = Vector2.zero;
    }
}
