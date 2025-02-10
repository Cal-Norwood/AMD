using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	[SerializeField] public float velocity;
	[SerializeField] public float damage;
    private Rigidbody rb;
    //this is for your projectile, let unity physics deal with most of it for you but do use an interface and some custome logic for dealing damage with different shell types and impact normals etc
    //to prove this works chuck a health component on a target and shoot it

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(SpawnTimer());
    }

    private void LateUpdate()
    {
        rb.AddForce(velocity * transform.up, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent health = other.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.TakeDamage?.Invoke(damage);
        }

        Destroy(gameObject);
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

}
