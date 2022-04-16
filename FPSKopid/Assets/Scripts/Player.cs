using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag == "EnemyBullet")
        {
            currentHealth -= Random.Range(0.5f, 20f);
        }
    }

    private void Update()
    {
        Debug.Log(currentHealth);
    }
}
