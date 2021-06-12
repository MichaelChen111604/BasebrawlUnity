using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour
{
    [Header("Health")] [Tooltip("Starting health")]
    public static float BaseHealth = 100;
    // Current health
    public float Health { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Health = BaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called to change Health
    void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    // Called when health is 0
    void Die()
    {
        Destroy(gameObject);
    }
}
