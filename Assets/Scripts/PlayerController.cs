using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(SpriteRenderer))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public Camera mainCamera;

    private SpriteRenderer sr;

    public float maxHealth = 100f;
    public float health;

    public GameObject knight;
    private KnightController knightController;
    public GameObject knightDamageMask;

    private KnightFormController knightFormController;

    public GameObject wizard;
    private WizardController wizardController;
    public GameObject wizardDamageMask;

    // true is the knight, false is the wizard
    private bool player = true;

    public GameObject knightUI;

    public GameObject wizardUI;

    public Vector2 lastDirection = Vector2.right;

    public bool invincible = false;

    private float damageCooldown = 0.5f;
    private float lastDamageTime = -999f;

    [SerializeField] private bool canSwapCharacters;

    public float headOffset = 0.5f;


    void Start()
    {
        knightController = knight.GetComponent<KnightController>();
        knightFormController = knight.GetComponent<KnightFormController>();
        wizardController = wizard.GetComponent<WizardController>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        knightDamageMask.SetActive(false);
        wizardDamageMask.SetActive(false);

        health = maxHealth;
        invincible = false;
        //canSwapCharacters = true;
    }


    void Update()
    {
        HandleCharacter();

        RegenerateAbilities();
    }
    void LateUpdate()
    {
       
    }

    void SortZByY()
    {
        // Sort the player based on their Y position (lower Y = higher Z)
        float zPosition = -transform.position.y;
    transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
}

void HandleCharacter()
    {
        if (!canSwapCharacters)
        {
            return;
        }
        if (Input.GetKeyDown(Bindings.SwapCharacter))
        {
            player = !player;

            knight.SetActive(player);
            wizard.SetActive(!player);

            knightUI.SetActive(player);
            wizardUI.SetActive(!player);

            // Ensure both characters align with the player position
            knight.transform.localPosition = Vector3.zero;
            wizard.transform.localPosition = Vector3.zero;
        }
    }

    void RegenerateAbilities()
    {
        // REGENERATE ENERGY
        // if current energy less than max energy
        if (knightController.currentEnergy < knightController.maxEnergy)
        {
            // regenerate energy
            knightController.currentEnergy += knightController.energyRegenRate * Time.deltaTime;
            // clamp to max energy
            if (knightController.currentEnergy > knightController.maxEnergy)
            {
                knightController.currentEnergy = knightController.maxEnergy;
            }
        }



        // REGENERATE MANA
        // if current mana less than max mana
        if (wizardController.currentMana < wizardController.maxMana)
        {
            // regenerate mana
            wizardController.currentMana += wizardController.manaRegenRate * Time.deltaTime;
            // clamp to max mana
            if (wizardController.currentMana > wizardController.maxMana)
            {
                wizardController.currentMana = wizardController.maxMana;
            }
        }
    }

    // TODO: refactor this so that it is handled only by the GameObject, dynamically
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if object is on Weapon layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // Try to get Weapon component
            Weapon weapon = other.GetComponent<Weapon>();

            if (weapon != null && !invincible)
            {
                HandleDamage(weapon.damage);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Fireball"))
        {
            Debug.Log("Hit by fireball");
            DragonFireball fireball = other.GetComponent<DragonFireball>();

            if (fireball != null && !invincible)
            {
                HandleDamage(fireball.damage);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("FireBreath"))
        {
            Debug.Log("Hit by fire breath");
            DragonFireBreath fireBreath = other.GetComponent<DragonFireBreath>();

            if (fireBreath != null && !invincible)
            {
                HandleDamage(fireBreath.damage);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Pincer"))
        {
            Debug.Log("Hit by pincer");
            SpiderPincerPrefab pincer = other.GetComponent<SpiderPincerPrefab>();

            if (pincer != null && !invincible) { 

                HandleDamage(pincer.damage);
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("SpiderStomp"))
        {
            Debug.Log("Hit by spider stomp");
            SpiderStomp stomp = other.GetComponent<SpiderStomp>();

            if (stomp != null && !invincible)
            {

                HandleDamage(stomp.damage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            Weapon weapon = other.GetComponent<Weapon>();

            if (weapon != null && !invincible)
            {
                if (Time.time >= lastDamageTime + damageCooldown)
                {
                    HandleDamage(weapon.damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    void HandleDamage(float incomingDamage)
    {
        health -= incomingDamage;
        // Debug.Log("Player took " + incomingDamage + " damage. Current health: " + health);

        if (health <= 0)
        {
            // Handle player death (e.g., play animation, disable controls, etc.)
            //Debug.Log("Player has died!");
        }

        // knight - damage mask
        //Debug.Log("calling color enum");
        StartCoroutine(ColorEnumerator());
    }

    public IEnumerator ColorEnumerator()
    {
        //Debug.Log("in color enum");
        if (player)
        {
            //Debug.Log("knight color enum");
            // fade out damage mask over 0.5 seconds
            knightDamageMask.SetActive(true);
            //Debug.Log("set active");
            float duration = 0.5f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / duration);
                Color knightColorRenderer = knightDamageMask.GetComponent<SpriteRenderer>().color;
                knightColorRenderer.a = alpha; // set alpha to fading value
                yield return null;
            }
            //Debug.Log("done iterating");
            knightDamageMask.SetActive(false);
        }
        else 
        {
            // fade out damage mask over 0.5 seconds
            wizardDamageMask.SetActive(true);
            float duration = 0.5f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0.5f, 0f, elapsed / duration);
                Color wizardColorRenderer = wizardDamageMask.GetComponent<SpriteRenderer>().color;
                wizardColorRenderer.a = alpha; // set alpha to fading value
                yield return null;
            }
            wizardDamageMask.SetActive(false);
        }
    }

    public void setCanSwapCharactersTrue() 
    {
        canSwapCharacters = true;
    }

    public void setCanSwapCharactersFalse()
    {
        canSwapCharacters = false;
    }
}
