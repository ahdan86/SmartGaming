using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private Transform targetTransform;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.UpdateHealthBar(maxHealth, currentHealth);
    }

    private void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag == "Bullet")
        {
            currentHealth -= Random.Range(0.5f, 20f);
            healthbar.UpdateHealthBar(maxHealth, currentHealth);
        }
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
