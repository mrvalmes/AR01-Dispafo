using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    [Tooltip("Vida Máxima")]
    public float maxHealth = 50f;
    [Tooltip("Máxima Resistencia")]
    public float maxShield = 200f;

    public float currentHealth { get; set; }
    public float currentShield { get; set; }
    public bool invincible { get; set; }

    private bool isShieldBroke;

    private PlayerController playerController;

    private bool m_IsDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        // Inicializar playerController
        playerController = FindObjectOfType<PlayerController>();
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (invincible || m_IsDead)
            return;

        // Determinar si reduce vida o armadura
        if (currentShield > 0)
        {
            currentShield -= damage;
            currentShield = Mathf.Clamp(currentShield, 0f, maxShield);
        }
        else
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        // Invocar evento de daño
        EventManager.current.DamageEnemyEvent.Invoke(currentHealth <= 0f);

        HandleShield();
        HandleDeath();
    }

    private void HandleShield()
    {
        if (!isShieldBroke && currentShield <= 0)
        {
            isShieldBroke = true;
            if (playerController != null)
            {
                playerController.Score(10);
                Debug.Log("Son 10 Puntos");
            }
            else
            {
                Debug.LogError("PlayerController is not assigned.");
            }
        }
    }

    public void Kill()
    {
        currentHealth = 0f;
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (m_IsDead)
            return;

        if (currentHealth <= 0f)
        {
            m_IsDead = true;
            Destroy(gameObject);
            Debug.Log("Son 100 Puntos Muerte");

            if (playerController != null)
            {
                playerController.Score(100);
            }
            else
            {
                Debug.LogError("PlayerController is not assigned.");
            }

            if (gameObject.CompareTag("Boss"))
            {
                Debug.Log("Murió el Boss");
                if (playerController != null)
                {
                    playerController.Victoria();
                }
                else
                {
                    Debug.LogError("PlayerController is not assigned.");
                }
            }
        }
    }
}