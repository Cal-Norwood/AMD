using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float MaxHealth;

    public Action<float> TakeDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = MaxHealth;
    }

    private void Awake()
    {
        TakeDamage += UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth(float damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
