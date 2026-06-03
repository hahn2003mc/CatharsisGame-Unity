using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public float minDamage = 6f;
    public float maxDamage = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = Random.Range(minDamage, maxDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
