using System.Collections.Generic;
using UnityEngine;

public class SlashAttackColliderController : MonoBehaviour
{
    public KnightController knightController; // Reference to the KnightController script
    public float damage;

    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    void OnEnable()
    {
        damage = knightController.slashAttackDamage; // Get the damage value from the KnightController
        hitTargets.Clear(); // reset every time the swing starts
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            hitTargets.Add(other.gameObject);

            Debug.Log("Hit: " + other.name);

            other.GetComponent<GruntController>().TakeDamage(damage);
        }
        if (other.CompareTag("Dragon") && !hitTargets.Contains(other.gameObject))
        {
            hitTargets.Add(other.gameObject);

            Debug.Log("Hit: " + other.name);

            other.GetComponent<DragonController>().TakeDamage(damage);
        }
        if (other.CompareTag("MotherSpider") && !hitTargets.Contains(other.gameObject))
        {
            hitTargets.Add(other.gameObject);

            Debug.Log("Hit: " + other.name);

            other.GetComponent<MotherSpiderController>().TakeDamage(damage);
        }
    }
}
