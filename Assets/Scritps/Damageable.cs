using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Tooltip("Multiplicador del daño")]
    public float damageMultiplier = 1f;
    public Health health { get; private set; }

    void Awake()
    {
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if (health)
        {
            float totalDamage = damage;

            if (!isExplosionDamage)
            {
                totalDamage *= damageMultiplier;
            }

            health.TakeDamage(totalDamage, damageSource);
        }
    }
}
