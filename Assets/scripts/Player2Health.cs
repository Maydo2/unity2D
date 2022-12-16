using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Health : MonoBehaviour
{
    public float maxHealth = 100;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;    
    }

    public void Player2TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(2);
    }
}
